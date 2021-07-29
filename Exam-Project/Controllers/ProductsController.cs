namespace Exam_Project.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

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

        

        

        public IActionResult All(ProductQueryViewModel product)
        {
            var productQuery = this.data.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(product.Name))
            {
                productQuery = productQuery.Where(p => p.Name == product.Name);
            }

            if (!string.IsNullOrWhiteSpace(product.SearchTerm))
            {
                productQuery = productQuery.Where(p => p.Name.ToLower().Contains(product.SearchTerm.ToLower()) ||
                                                  p.Description.ToLower().Contains(product.SearchTerm.ToLower()));
            }
            if (product.CategoryId != 0)
            {
                productQuery = productQuery.Where(p => p.CategoryId == product.CategoryId);
            }

            productQuery = product.Sorting switch
            {
                Sorting.Price => productQuery.OrderBy(p => p.Price),
                Sorting.Name => productQuery.OrderBy(p => p.Name),
                Sorting.Latest or _ => productQuery.OrderByDescending(p => p.Id),
            };



            var allProducts = this.data.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                OrderingNumber = p.OrderingNumber,
                CategoryId = p.CategoryId
            }).ToList();

            return View(allProducts);
        }

        
    }
}
