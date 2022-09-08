namespace OnlineShop.Services
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Data;
    using OnlineShop.Models.Products;
    using OnlineShop.Services.Models;

    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICategoryService _categoryService;

        public ProductService(ApplicationDbContext dbContext, ICategoryService categoryService)
        {
            _dbContext = dbContext;
            _categoryService = categoryService;
        }
        public async Task<AllProductsDto> GetAllProducts(AllProductsDto query, CancellationToken cancellationToken)
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
                        OrderingNumber = p.OrderingNumber,
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
        public async Task<ProductDto> GetProductById(int productId, CancellationToken cancellationToken)
            => await _dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    OrderingNumber = p.OrderingNumber,
                    CategoryId = p.CategoryId,
                    Category = p.Category.Name
                })
                .FirstOrDefaultAsync(cancellationToken);



    }
}
