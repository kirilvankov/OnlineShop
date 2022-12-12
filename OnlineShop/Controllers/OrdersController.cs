namespace OnlineShop.Controllers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Data.Infrastructure;
    using OnlineShop.Models.Orders;
    using OnlineShop.Services;

    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [Authorize]
        public async Task<IActionResult> MyOrders(CancellationToken cancellationToken)
        {
            var userId = this.User.GetId();

            var res = await _orderService.GetUserOrders(userId, cancellationToken);

            var result = new AllOrdersViewModel();
            result.Orders = res.Select(o => new OrderViewModel
            {
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(oi => new OrderItemsViewModel
                {
                    Id = oi.Id,
                    Name = oi.Name,
                    ProductId = oi.ProductId,
                    ProductPrice = oi.ProductPrice,
                    ImageUrl = oi.ImageUrl,
                    Quantity = oi.Quantity
                }).ToList(),
                TotalPrice = o.Items.Sum(oi => oi.ProductPrice)
            }).ToList();
            
            return View(result);
        }
    }
}
