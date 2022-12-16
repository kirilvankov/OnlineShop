namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Data.Enums;
    using OnlineShop.Services.Models;

    public interface IStoreService
    {
        Task<int> Apply(RegisterStoreDto model, string userId, CancellationToken cancellationToken);

        Task<List<StoreDto>> GetStores(CancellationToken cancellationToken);

        Task<int?> GetStoreId(string userId, CancellationToken cancellationToken);

        Task<StoreDto> GetStore(int storeId, CancellationToken cancellationToken);

        Task<Status> ApproveStore(int storeId, CancellationToken cancellationToken);

        Task<Status> RejectStore(int storeId, CancellationToken cancellationToken);
    }
}
