namespace OnlineShop.Services
{
    using System;
    using System.IO;
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

        public ProductService(ApplicationDbContext dbContext,
            ICategoryService categoryService,
            IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _categoryService = categoryService;
            _environment = environment;
        }

        public async Task AddAsync(ProductFormModel product, int? storeId, CancellationToken cancellationToken)
        {
            string imageUrl = product.ImageUrl;
            if (product.Image != null)
            {
                imageUrl = UploadFile(product);
            }
            
            var productData = new ProductEntity
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = imageUrl,
                CategoryId = product.CategoryId,
                StoreId = storeId,
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

            var imageUrl = product.ImageUrl;
            if (product.Image != null)
            {
                if (imageUrl.StartsWith("/media/"))
                {
                    var imageName = imageUrl.Replace("/media/", string.Empty);
                    var uploadedFolder = Path.Combine(_environment.WebRootPath, "media");
                    string filePath = Path.Combine(uploadedFolder, imageName);

                    DeleteFile(filePath);
                }
                
                imageUrl = UploadFile(product);
            }
            else
            {
                if (editedProduct.ImageUrl.StartsWith("/media/") && imageUrl != editedProduct.ImageUrl)
                {
                    var imageName = editedProduct.ImageUrl.Replace("/media/", string.Empty);
                    var uploadedFolder = Path.Combine(_environment.WebRootPath, "media");
                    string filePath = Path.Combine(uploadedFolder, imageName);
                    
                    DeleteFile(filePath);
                }
            }

            editedProduct.Name = product.Name;
            editedProduct.Description = product.Description;
            editedProduct.Price = product.Price;
            editedProduct.ImageUrl = imageUrl;
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
        { 
            var productEntity = await _dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => new 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    Category = p.Category.Name,
                    StoreId = p.StoreId,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (productEntity == null)
            {
                return null;
            }
            return new ProductDto()
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                Price = productEntity.Price,
                ImageUrl = productEntity.ImageUrl,
                CategoryId = productEntity.CategoryId,
                Category = productEntity.Category,
                StoreId = productEntity?.StoreId
            };
        }
            


        public async Task DeleteAsync(int id, CancellationToken cancellation)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            string imageUrl = product.ImageUrl;
            string filePath = string.Empty;
            if (imageUrl.StartsWith("/media/"))
            {
                var imageName = imageUrl.Replace("/media/", string.Empty);
                var uploadedFolder = Path.Combine(_environment.WebRootPath, "media");
                filePath = Path.Combine(uploadedFolder, imageName);
            }

            _dbContext.Products.Remove(product);

            DeleteFile(filePath);

            await _dbContext.SaveChangesAsync();
        }


        private string UploadFile(ProductFormModel product)
        {
            string imageUrl = null;

            if (product.Image != null)
            {
                var uploadedFolder = Path.Combine(_environment.WebRootPath, "media");
                string imageFile = Guid.NewGuid().ToString() + "_" + product.Image.FileName;
                string filePath = Path.Combine(uploadedFolder, imageFile);

                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        product.Image.CopyTo(fs);
                    }
                    imageUrl = "/media/" + imageFile;
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.Message);
                }
            }

            return imageUrl;
        }
        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.Message);
                }
            }
        }
    }
}
