namespace OnlineShop.Areas.ShopOwner.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(ShopOwnerConstants.AreaName)]
    [Authorize(Roles = ShopOwnerConstants.RoleName)]
    public class BaseShopOwnerController : Controller
    {
    }
}
