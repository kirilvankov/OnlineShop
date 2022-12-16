namespace OnlineShop.Areas.Admin.Controllers.Api
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Areas.Admin.Models.Store;
    using OnlineShop.Data.Enums;
    using OnlineShop.Services;

    [Area(AdminConstants.AreaName)]
    [Authorize(Roles = (AdminConstants.AdministratorRoleName))]
    [Route("api/[controller]")]
    [ApiController]
    public class ManageController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public ManageController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpPost]
        public async Task<ManageStoreResponceModel> Approve(AdminMenageStore input, CancellationToken cancellationToken)
        {
            Status result = Status.None;
            if (input.Action == "approve")
            {
                result = await _storeService.ApproveStore(input.StoreId, cancellationToken);
            }
            else if(input.Action == "notApprove")
            {
                result = await _storeService.RejectStore(input.StoreId, cancellationToken);
            }

            var model = new ManageStoreResponceModel() { Status = result.ToString() };
            return model;
        }
    }
}
