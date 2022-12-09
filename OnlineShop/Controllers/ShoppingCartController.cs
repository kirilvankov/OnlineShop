namespace OnlineShop.Controllers
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Data.Infrastructure;
    using OnlineShop.Models.Address;
    using OnlineShop.Models.Orders;
    using OnlineShop.Models.User;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;

    public class ShoppingCartController : Controller
    {
        private readonly IShopppingCartService _shoppingCartService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ShoppingCartController(IShopppingCartService shoppingCartService, IUserService userService, IMapper mapper)
        {
            _shoppingCartService = shoppingCartService;
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            CartServiceModel model = _shoppingCartService.GetCurrentCart();
            return View(model);
        }
        public IActionResult Add(int id)
        {
            var result = _shoppingCartService.AddItem(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var result = _shoppingCartService.RemoveItem(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrease(int id)
        {
            var result = _shoppingCartService.Decrease(id);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
        {
            var userId = User.GetId();
            var tempAddress = _shoppingCartService.GetOrderAddress();
            var userDetails = await _userService.GetUserDetails(userId, cancellationToken);

            AddressInfoDto orderAddress = null;

            if (tempAddress == null && userDetails.AddressInfo?.AddressLine1 == null)
            {
                return RedirectToAction(nameof(Address));
            }

            if (userDetails.AddressInfo?.AddressLine1 != null)
            {
                orderAddress = userDetails.AddressInfo;
            }

            if (tempAddress != null)
            {
                orderAddress = tempAddress;
            }

            CartServiceModel cart = _shoppingCartService.GetCurrentCart();

            if (cart.StoredItems.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new PreviewOrderViewModel()
            {
                Cart = cart,
                UserDetails = new OrderUserViewModel()
                {
                    UserId = userId,
                    FirstName = userDetails.FirstName,
                    LastName = userDetails.LastName,
                    AddressInfo = _mapper.Map<AddressInfoViewModel>(orderAddress),
                    
                }
            };

            if (viewModel.UserDetails.AddressInfo != null)
            {
                _shoppingCartService.SetOrderAddress(orderAddress);
            }
            
            return View(viewModel);
        }

        [Authorize]
        public IActionResult Address()
        {

            var address = _shoppingCartService.GetOrderAddress();
            
            if (address != null)
            {
                var inputModel = _mapper.Map<AddressInfoInputModel>(address);
                return View(inputModel);
            }
            
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Address(AddressInfoInputModel input)
        {
            var userId = User.GetId();
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var model = _mapper.Map<AddressInfoDto>(input); 

            if (input.IsUserAddress)
            {
                _userService.SetUserAddress(model, userId);
                _shoppingCartService.SetOrderAddress(model);
            }
            else
            {
                _shoppingCartService.SetOrderAddress(model);
            }
            
            return RedirectToAction(nameof(Checkout));
        }
    }
}
