namespace OnlineShop.Areas.ShopOwner.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(ShopOwnerConstants.AriaName)]
    [Authorize(Roles = ShopOwnerConstants.RoleName)]
    public class BaseShopOwnerController : Controller
    {
    }
}
