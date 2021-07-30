namespace Exam_Project.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Exam_Project.Areas.Admin.Models.Products;
    using Exam_Project.Data;
    using Exam_Project.Data.Models;
    using Exam_Project.Models.Products;

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
            var products = this.data.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                OrderingNumber = p.OrderingNumber,
                CategoryId = p.CategoryId
            }).ToList();

            return View(products);
        }
        public IActionResult Add()
        {

            return View(new ProductFormModel
            {
                Categories = GetCategories()
            });
        }

        [HttpPost]
        public IActionResult Add(ProductFormModel product)
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
        public IActionResult Edit(int id)
        {
            var product = this.data.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductFormModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Categories = GetCategories()
            });
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductFormModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var editedProduct = this.data.Products.Where(p => p.Id == id).FirstOrDefault();

            editedProduct.Name = product.Name;
            editedProduct.Description = product.Description;
            editedProduct.Price = product.Price;
            editedProduct.ImageUrl = product.ImageUrl;
            editedProduct.CategoryId = product.CategoryId;

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
