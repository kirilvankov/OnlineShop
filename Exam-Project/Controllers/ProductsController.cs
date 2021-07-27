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

        public IActionResult Add()
        {

            return View(new AddProductFormModel
            {
                Categories = GetCategories()
            });
        }

        [HttpPost]
        public IActionResult Add(AddProductFormModel product)
        {
            if (!CategoryExist(product.CategoryId))
            {
                ModelState.AddModelError("Category", "Invalid Category");
            }
            if (!ModelState.IsValid)
            {
                product.Categories = GetCategories();
                return View(product);
            }

            var productData = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };

            return RedirectToAction(nameof(All));

        }
        public IActionResult All()
        {
            return View();
        }

        private IEnumerable<ProductCategoriesViewModel> GetCategories()
            => this.data.Categories.Select(c => new ProductCategoriesViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId
            }).ToList();

        private bool CategoryExist(int categoryId)
            => this.data.Categories.Any(c => c.Id == categoryId);
    }
}
