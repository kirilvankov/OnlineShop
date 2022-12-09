namespace OnlineShop.Areas.Admin.Controllers.Api
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Areas.Admin.Models.Orders;
    using OnlineShop.Data.Infrastructure;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IShopppingCartService _shopppingCart;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IShopppingCartService shopppingCart, IUserService userService)
        {
            _orderService = orderService;
            _shopppingCart = shopppingCart;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(TransactionInputModel input, CancellationToken cancellationToken)
        {
            var userId = User.GetId();
            var products = _shopppingCart.GetCurrentCart();
            var tempAddress = _shopppingCart.GetOrderAddress();
            var userDetails = await _userService.GetUserDetails(userId, cancellationToken);

            var OrderAddress = new AddressInfoDto();
            if (tempAddress != null)
            {
                OrderAddress = tempAddress;
            }
            else
            {
                OrderAddress = userDetails.AddressInfo;
            }

            var model = new CreateOrderInputModelDto()
            {
                UserId = userId,
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                TotalPrice = products.TotalPrice,
                Transaction = input,
                Products = products.StoredItems,
                AddressInfo = OrderAddress,
            };
           var result = await _orderService.CreateOrder(model, cancellationToken);

           _shopppingCart.ClearCart();
           return CreatedAtAction("CreateOrder", new { id = result });
        }
    }
}
