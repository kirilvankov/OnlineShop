namespace OnlineShop.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Areas.Admin.Models.Products;
    using OnlineShop.Services.Models;

    public interface IProductService
    {
        Task<AllProductsDto> GetAllAsync(AllProductsDto query, CancellationToken cancellationToken);

        Task<AllProductsDto> GetAllPerStoreAsync(AllProductsDto query, int? storeId, CancellationToken cancellationToken);

        Task<ProductDto> GetByIdAsync(int productId, CancellationToken cancellationToken);

        Task AddAsync(ProductFormModel product, int? storeId, CancellationToken cancellationToken);

        Task<int?> EditAsync(int id, ProductFormModel product, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
