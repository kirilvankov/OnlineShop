 namespace OnlineShop.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

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
    }
}
