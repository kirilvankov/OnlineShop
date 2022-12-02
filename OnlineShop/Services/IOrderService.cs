namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Services.Models;

    public interface IOrderService
    {
        Task<List<OrderDto>> GetUserOrders(string userId, CancellationToken cancellationToken);
        void CreateOrder(string userId, CartServiceModel input, CancellationToken cancellationToken);
    }
}
