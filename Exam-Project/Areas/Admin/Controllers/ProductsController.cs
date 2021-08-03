namespace Exam_Project.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Exam_Project.Areas.Admin.Models.Products;
    using Exam_Project.Data;
    using Exam_Project.Data.Models;
    using Exam_Project.Models.Products;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using static System.Net.Mime.MediaTypeNames;

    public class ProductsController : BaseAdminController
    {
        private readonly ProjectDbContext data;
        private readonly IWebHostEnvironment env;

        public ProductsController(ProjectDbContext data, IWebHostEnvironment env)
        {
            this.data = data;
            this.env = env;
        }

        public IActionResult All([FromQuery] AdminProductViewModel query)
        {
            var productsQuery = this.data.Products.AsQueryable();
            var totalItems = this.data.Products.Count();
            var allProducts = productsQuery
                .Skip((query.CurrentPage - 1) * query.ItemPerPage)
                .Take(query.ItemPerPage)
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
                CurrentPage = query.CurrentPage,
                ItemPerPage = query.ItemPerPage,
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
        public async Task<IActionResult> Add(ProductFormModel product /*, IFormFile file*/)
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

            var editedProduct = this.data.Products.Where(p => p.Id == id).FirstOrDefault();

            if (product.Image != null)
            {
                var uploadedFolder = Path.Combine(this.env.WebRootPath, "media");
                var filePath = Path.Combine(uploadedFolder, product.ImageName);

                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);

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

        private string UploadFile(ProductFormModel product)
        {
            string imageFile = null;

            if (product.Image != null)
            {
                var uploadedFolder = Path.Combine(this.env.WebRootPath, "media");
                imageFile = Guid.NewGuid().ToString() + "_" + product.Image.FileName;

                var filePath = Path.Combine(uploadedFolder, imageFile);

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    product.Image.CopyTo(fs);
                }
            }

            return imageFile;
        }
    }
}
