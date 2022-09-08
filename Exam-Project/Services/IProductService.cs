namespace Exam_Project.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using Exam_Project.Services.Models;

    public interface IProductService
    {
        Task<AllProductsDto> GetAllProducts(AllProductsDto query, CancellationToken cancellationToken);
        Task<ProductDto> GetProductById(int productId, CancellationToken cancellationToken);
    }
}
