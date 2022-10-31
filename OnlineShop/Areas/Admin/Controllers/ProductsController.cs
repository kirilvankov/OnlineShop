namespace OnlineShop.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using FileSystem = System.IO;
    using System.Linq;
    using OnlineShop.Areas.Admin.Models.Products;
    using OnlineShop.Data;
    using OnlineShop.Data.Models;
    using OnlineShop.Models.Products;

    using Microsoft.AspNetCore.Hosting;

    using Microsoft.AspNetCore.Mvc;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;
    using System.Threading;
    using System.Threading.Tasks;

    public class ProductsController : BaseAdminController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;

        public ProductsController(ApplicationDbContext dbContext, IWebHostEnvironment env, IProductService productService)
        {
            _dbContext = dbContext;
            _env = env;
            _productService = productService;
        }

        public async Task<IActionResult> All([FromQuery] LoadProductsRequest request, CancellationToken cancellationToken)
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

            var model = new PagedResult<ProductViewModel>
            {
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                SearchTerm = result.SearchTerm,
                TotalCount = result.TotalItems,
                Sorting = result.Sorting,
                CategoryId = result.CategoryId,
                Products = result.Products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    OrderingNumber = p.OrderingNumber,
                }).ToList(),
                Categories = result.Categories.Select(c => new ProductCategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentId = c.ParentId
                }).ToList()

            };

            return View(model);
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


            var productData = new ProductEntity
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

            var fileName = product.ImageName;
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
                fileName = UploadFile(product);
            }
            

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
