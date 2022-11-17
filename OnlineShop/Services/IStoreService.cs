namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Services.Models;

    public interface IStoreService
    {
        Task<int> Apply(RegisterStoreDto model, string userId, CancellationToken cancellationToken);
        Task<List<StoreDto>> GetStores(CancellationToken cancellationToken);
        Task<int?> GetStoreId(string userId, CancellationToken cancellationToken);
    }
}
