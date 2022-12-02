namespace OnlineShop.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Models.Address;
    using OnlineShop.Models.Store;
    using OnlineShop.Services;

    public class StoreController : BaseAdminController
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await _storeService.GetStores(cancellationToken);
            var model = new AllStoresViewModel()
            {
                Stores = result.Select(s => new StoreViewModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    AdditionalInfo = s.AdditionalDetails,
                    Status = s.Status,
                }).ToList(),
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int storeId, CancellationToken cancellationToken)
        {
            var storeDto = await _storeService.GetStore(storeId, cancellationToken);

            if (storeDto == null)
            {
                return NotFound();
            }
            var result = new StoreDetailsViewModel() 
            { 
                Id = storeDto.Id,
                Name = storeDto.Name,
                Description = storeDto.Description,
                AdditionalInfo = storeDto.AdditionalDetails,
                Status = storeDto.Status,
                UserId = storeDto.UserId,
                AddressInfo = new AddressInfoInputModel()
                {
                    AddressLine1 = storeDto.AddressInfo.AddressLine1,
                    AddressLine2 = storeDto.AddressInfo.AddressLine2,
                    City = storeDto.AddressInfo.City,
                    PhoneNumber = storeDto.AddressInfo.PhoneNumber,
                    PostCode = storeDto.AddressInfo.PostCode,
                    Email = storeDto.AddressInfo.Email,
                    LocationLat = storeDto.AddressInfo.LocationLat,
                    LocationLng = storeDto.AddressInfo.LocationLng,
                }
            };

            return View(result);
        }

    }
}
