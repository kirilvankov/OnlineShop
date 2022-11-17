namespace OnlineShop.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Data.Infrastructure;
    using OnlineShop.Models.Store;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;

    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Apply(RegisterStoreFormModel input, CancellationToken cancellationToken)
        {
            var userEmail = User.GetEmail();
            var userId = User.GetId();
            if (userEmail != input.AddressInfo.Email)
            {
                ModelState.AddModelError("Email", errorMessage: "The Email should be same as signed in.");
            }
            var storeId = await _storeService.GetStoreId(userId, cancellationToken);
            if (storeId != null)
            {
                ModelState.AddModelError("Name", "You had already created Store");
            }
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var dto = new RegisterStoreDto()
            {
                Name = input.Name,
                Description = input.Description,
                AdditionalDetails = input.AdditionalDetails,
                AddressInfo = new AddressInfoDto()
                {
                    AddressLine1 = input.AddressInfo.AddressLine1,
                    AddressLine2 = input.AddressInfo.AddressLine2,
                    PhoneNumber = input.AddressInfo.PhoneNumber,
                    City = input.AddressInfo.City,
                    Email = input.AddressInfo.Email,
                    PostCode = input.AddressInfo.PostCode,
                    LocationLat = input.AddressInfo.LocationLat,
                    LocationLng = input.AddressInfo.LocationLng,
                }
            };

            var res = await _storeService.Apply(dto, userId, cancellationToken);
            
            return RedirectToAction(nameof(ApplyResult), new { result = res });
        }

        public IActionResult ApplyResult(int? result)
        {
            ViewData["Success"] = result;
            return View();
        }
        

    }
}
