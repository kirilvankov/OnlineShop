namespace Exam_Project.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Exam_Project.Areas.Admin.Models.Products;
    using Exam_Project.Data;
    using Exam_Project.Data.Models;
    using Exam_Project.Models.Products;

    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : Controller
    {
        private readonly ProjectDbContext data;

        public ProductsController(ProjectDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery]ProductQueryViewModel query)
        {
            var queryProducts = this.data.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                queryProducts = queryProducts.Where(p => p.Name == query.Name);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                queryProducts = queryProducts.Where(p => p.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                                                  p.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }
            if (query.CategoryId != null)
            {
                queryProducts = queryProducts.Where(p => p.CategoryId == query.CategoryId);
            }

            queryProducts = query.Sorting switch
            {
                Sorting.Price => queryProducts.OrderBy(p => p.Price),
                Sorting.Name => queryProducts.OrderBy(p => p.Name),
                Sorting.Latest or _ => queryProducts.OrderByDescending(p => p.Id),
            };

            

            var totalItems = queryProducts.Count();

            var allProducts = queryProducts
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        OrderingNumber = p.OrderingNumber,
                        CategoryId = p.CategoryId
                    }).ToList();

            return View(new ProductQueryViewModel
            {
                Products = allProducts,
                TotalItems = totalItems,
                PageIndex = query.PageIndex,
                SearchTerm = query.SearchTerm,
                Sorting = query.Sorting,
                CategoryId = query.CategoryId,
                PageSize = query.PageSize,
                Categories = GetCategories()
            });

        }
        public IActionResult Details(int id)
        {
            var product = this.data.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            var productCategory = this.data.Categories.Find(product.CategoryId);

            return View(new DetailsProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Category = productCategory.Name
            });
        }

        private IEnumerable<ProductCategoriesViewModel> GetCategories()
            => this.data.Categories.Select(c => new ProductCategoriesViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId
            }).ToList();
    }
}
