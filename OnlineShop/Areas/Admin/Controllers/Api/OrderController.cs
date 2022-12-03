namespace OnlineShop.Areas.Admin.Controllers.Api
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Areas.Admin.Models.Orders;
    using OnlineShop.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IShopppingCartService _shopppingCart;

        public OrderController(IOrderService orderService, IShopppingCartService shopppingCart)
        {
            _orderService = orderService;
            _shopppingCart = shopppingCart;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderInputModel input, CancellationToken cancellationToken)
        {

           var result = await _orderService.CreateOrder(input, cancellationToken);
           _shopppingCart.ClearCart();
           return CreatedAtAction("CreateOrder", new { id = result });
        }
    }
}
