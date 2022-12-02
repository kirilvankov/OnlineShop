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
        private readonly IShoppingCartStorage _storage;
        private readonly IMapper _mapper;

        public ShoppingCartController(IShopppingCartService shoppingCartService, IUserService userService, IShoppingCartStorage storage, IMapper mapper)
        {
            _shoppingCartService = shoppingCartService;
            _userService = userService;
            _storage = storage;
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
        public async Task<IActionResult> Preview(CancellationToken cancellationToken)
        {
            var userId = User.GetId();
            var tempAddress = _storage.GetOrderAddress();
            var userDetails = await _userService.GetUserDetails(userId, cancellationToken);
            if (tempAddress != null)
            {
                userDetails.AddressInfo = tempAddress;
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
                    FirstName = userDetails.FirstName,
                    LastName = userDetails.LastName,
                    AddressInfo = new AddressInfoViewModel()
                    {
                        AddressLine1 = userDetails.AddressInfo.AddressLine1,
                        AddressLine2 = userDetails.AddressInfo.AddressLine2,
                        PhoneNumber = userDetails.AddressInfo.PhoneNumber,
                        City = userDetails.AddressInfo.City,
                        Email = userDetails.AddressInfo.Email,
                        PostCode = userDetails.AddressInfo.PostCode,
                        
                    }
                }
            };

            if (viewModel.UserDetails.AddressInfo.AddressLine1 != null)
            {
                TempData["EditAddress"] = JsonSerializer.Serialize(viewModel.UserDetails.AddressInfo);
            }
            
            return View(viewModel);
        }

        [Authorize]
        public IActionResult Address()
        {
            string modelAsString = null;
            if (TempData["EditAddress"] != null)
            {
                modelAsString = TempData["EditAddress"].ToString();
            }
            
            if (!string.IsNullOrWhiteSpace(modelAsString))
            {
                var editModel = JsonSerializer.Deserialize<AddressInfoViewModel>(modelAsString);
                var model = _mapper.Map<AddressInfoInputModel>(editModel);
                return View(model);
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

            var model = new AddressInfoDto()
            {
                AddressLine1 = input.AddressLine1,
                AddressLine2 = input.AddressLine2,
                PhoneNumber = input.PhoneNumber,
                City = input.City,
                PostCode = input.PostCode,
                Email = input.Email,
                IsUserAddress = input.IsUserAddress,
            };

            if (input.IsUserAddress)
            {
                _userService.SetUserAddress(model, userId);
            }
            else
            {
                _storage.SetOrderAddress(model);
            }
            
            return RedirectToAction(nameof(Preview));
        }

        [Authorize]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Payment(object o)
        {
            return View();
        }
    }
}
