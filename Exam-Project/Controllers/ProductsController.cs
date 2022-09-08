﻿namespace Exam_Project.Controllers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Exam_Project.Areas.Admin.Models.Products;
    using Exam_Project.Models.Products;
    using Exam_Project.Services;
    using Exam_Project.Services.Models;

    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> All([FromQuery]LoadProductsRequest request, CancellationToken cancellationToken)
        {
            var query = new AllProductsDto
            {
                SearchTerm = request.SearchTerm,
                Sorting = request.Sorting,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                CategoryId = request.CategoryId,
                TotalItems = request.TotalItems,
            };

            var result = await _productService.GetAllProducts(query, cancellationToken);

            

            return View(new ProductListViewModel
            {
                Products = result.Products.Select(p=> new ProductViewModel 
                { 
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    OrderingNumber = p.OrderingNumber,
                }),
                TotalItems = result.TotalItems,
                PageIndex = result.PageIndex,
                SearchTerm = result.SearchTerm,
                Sorting = result.Sorting,
                CategoryId = result.CategoryId,
                PageSize = result.PageSize,
                Categories = result.Categories.Select(c => new ProductCategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentId = c.ParentId
                }).ToList()
            });

        }
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductById(id, cancellationToken);
            if (product == null)
            {
                return NotFound();
            }

            return View(new DetailsProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Category = product.Category
            });
        }
    }
}
