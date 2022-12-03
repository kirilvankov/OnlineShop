namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Areas.Admin.Models.Orders;
    using OnlineShop.Services.Models;

    public interface IOrderService
    {
        Task<List<OrderDto>> GetUserOrders(string userId, CancellationToken cancellationToken);
        Task<int> CreateOrder(CreateOrderInputModel input, CancellationToken cancellationToken);
    }
}
