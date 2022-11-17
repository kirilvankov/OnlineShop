namespace OnlineShop.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Areas.Admin.Models.Products;
    using OnlineShop.Data;
    using OnlineShop.Data.Models;
    using OnlineShop.Models.Products;
    using OnlineShop.Services.Models;

    using FileSystem = System.IO;

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public ProductService(ApplicationDbContext dbContext, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _categoryService = categoryService;
            _environment = environment;
        }

        public async Task AddAsync(ProductFormModel product, CancellationToken cancellationToken)
        {
            var fileName = UploadFile(product);


            var productData = new ProductEntity
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = fileName,
                CategoryId = product.CategoryId
            };

            await _dbContext.AddAsync(productData, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int?> EditAsync(int id, ProductFormModel product, CancellationToken cancellationToken)
        {
            var editedProduct = await _dbContext.Products.FindAsync(id);

            if (editedProduct == null)
            {
                return null;
            }

            var fileName = product.ImageName;
            if (product.Image != null)
            {
                var uploadedFolder = FileSystem.Path.Combine(_environment.WebRootPath, "media");
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

            await _dbContext.SaveChangesAsync(cancellationToken);
            return editedProduct.Id;
        }

        public async Task<AllProductsDto> GetAllAsync(AllProductsDto query, CancellationToken cancellationToken)
        {
            var queryProducts = _dbContext.Products.AsQueryable();

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
            var totalItemsCount = queryProducts.Count();

            var allCategories = await _categoryService.GetAllCategories(cancellationToken);

            var allProducts = await queryProducts
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        CategoryId = p.CategoryId
                    }).ToListAsync();

            var result = new AllProductsDto
            {
                SearchTerm = query.SearchTerm,
                Sorting = query.Sorting,
                CategoryId = query.CategoryId,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalItems = totalItemsCount,
                Products = allProducts,
                Categories = allCategories
            };
            return result;
        }

        public async Task<AllProductsDto> GetAllPerStoreAsync(AllProductsDto query, int? storeId, CancellationToken cancellationToken)
        {
            var queryProducts = _dbContext.Products.Where(p => p.StoreId == storeId).AsQueryable();

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
            var totalItemsCount = queryProducts.Count();

            var allCategories = await _categoryService.GetAllCategories(cancellationToken);

            var allProducts = await queryProducts
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        CategoryId = p.CategoryId
                    }).ToListAsync();

            var result = new AllProductsDto
            {
                SearchTerm = query.SearchTerm,
                Sorting = query.Sorting,
                CategoryId = query.CategoryId,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalItems = totalItemsCount,
                Products = allProducts,
                Categories = allCategories
            };
            return result;
        }

        public async Task<ProductDto> GetByIdAsync(int productId, CancellationToken cancellationToken)
            => await _dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    Category = p.Category.Name
                })
                .FirstOrDefaultAsync(cancellationToken);


        private string UploadFile(ProductFormModel product)
        {
            string imageFile = null;

            if (product.Image != null)
            {
                var uploadedFolder = FileSystem.Path.Combine(_environment.WebRootPath, "media");
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
