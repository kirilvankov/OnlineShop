namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Services.Models;

    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategories(CancellationToken cancellationToken);
        bool CategoryExist(int categoryId);
    }
}
