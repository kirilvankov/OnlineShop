namespace Exam_Project.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using FileSystem = System.IO;
    using System.Linq;
    using Exam_Project.Areas.Admin.Models.Products;
    using Exam_Project.Data;
    using Exam_Project.Data.Models;
    using Exam_Project.Models.Products;

    using Microsoft.AspNetCore.Hosting;

    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : BaseAdminController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public IActionResult All([FromQuery] AdminProductViewModel query)
        {
            var productsQuery = _dbContext.Products.AsQueryable();
            var totalItems = _dbContext.Products.Count();
            var allProducts = productsQuery
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
                    CategoryId = p.CategoryId,

                }).ToList();

            return View(new AdminProductViewModel
            {
                TotalItems = totalItems,
                Products = allProducts,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                Categories = GetCategories()
            });
        }
        public IActionResult Add()
        {

            return View(new ProductFormModel
            {
                Categories = GetCategories()
            });
        }

        [HttpPost]
        public IActionResult Add(ProductFormModel product /*, IFormFile file*/)
        {
            if (product.Image == null)
            {
                ModelState.AddModelError("Image", "Field image is required!");
            }

            if (product.Image != null && product.Image.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "File is too large!");
            }


            if (!CategoryExist(product.CategoryId))
            {
                ModelState.AddModelError("Category", "Invalid Category");
            }
            if (!ModelState.IsValid)
            {
                product.Categories = GetCategories();
                return View(product);
            }

            var fileName = UploadFile(product);


            var productData = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = fileName,
                CategoryId = product.CategoryId
            };

            _dbContext.Add(productData);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(All));

        }
        public IActionResult Edit(int id)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }



            return View(new ProductFormModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageName = product.ImageUrl,
                CategoryId = product.CategoryId,
                Categories = GetCategories()
            });
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductFormModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(new ProductFormModel
                {
                    Categories = GetCategories()
                });
            }

            var editedProduct = _dbContext.Products.Where(p => p.Id == id).FirstOrDefault();

            if (editedProduct == null)
            {
                return NotFound();
            }

            if (product.Image != null)
            {
                var uploadedFolder = FileSystem.Path.Combine(_env.WebRootPath, "media");
                var filePath = FileSystem.Path.Combine(uploadedFolder, product.ImageName);

                if (FileSystem.File.Exists(filePath))
                {
                    try
                    {
                        FileSystem.File.Delete(filePath);

                    }
                    catch (Exception e)
                    {

                        throw new ArgumentException(e.Message);
                    }
                }

            }
            var fileName = UploadFile(product);

            editedProduct.Name = product.Name;
            editedProduct.Description = product.Description;
            editedProduct.Price = product.Price;
            editedProduct.ImageUrl = fileName;
            editedProduct.CategoryId = product.CategoryId;

            _dbContext.SaveChanges();

            return RedirectToAction(nameof(All));

        }

        //TODO: Create delete product action!
        private IEnumerable<ProductCategoriesViewModel> GetCategories()
            => _dbContext.Categories.Select(c => new ProductCategoriesViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId
            }).ToList();

        private bool CategoryExist(int categoryId)
                => _dbContext.Categories.Any(c => c.Id == categoryId);

        private string UploadFile(ProductFormModel product)
        {
            string imageFile = null;

            if (product.Image != null)
            {
                var uploadedFolder = FileSystem.Path.Combine(_env.WebRootPath, "media");
                imageFile = Guid.NewGuid().ToString() + "_" + product.Image.FileName;

                var filePath = FileSystem.Path.Combine(uploadedFolder, imageFile);

                using (FileSystem.FileStream fs = new FileSystem.FileStream(filePath, FileSystem.FileMode.Create))
                {
                    product.Image.CopyTo(fs);
                }
            }

            return imageFile;
        }
    }
}
