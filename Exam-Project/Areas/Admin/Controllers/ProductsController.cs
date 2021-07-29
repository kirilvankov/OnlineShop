namespace Exam_Project.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Exam_Project.Areas.Admin.Models.Products;
    using Exam_Project.Data;
    using Exam_Project.Data.Models;

    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : BaseAdminController
    {
        private readonly ProjectDbContext data;

        public ProductsController(ProjectDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            return View();
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

            this.data.Add(productData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));

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
