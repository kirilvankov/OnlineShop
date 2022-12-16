namespace OnlineShop.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Models.Address;
    using OnlineShop.Models.Store;
    using OnlineShop.Services;

    public class StoreController : BaseAdminController
    {
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public StoreController(IStoreService storeService, IMapper mapper)
        {
            _storeService = storeService;
            _mapper = mapper;
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
                AddressInfo = _mapper.Map<AddressInfoViewModel>(storeDto.AddressInfo)
            };

            return View(result);
        }
    }
}
