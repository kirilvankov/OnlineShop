namespace OnlineShop.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Data.Infrastructure;
    using OnlineShop.Models.Store;

    public class StoreController : Controller
    {
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
        public IActionResult Apply(RegisterStoreFormModel input)
        {
            var userEmail = User.GetEmail();
            if (userEmail != input.AddressInfo.Email)
            {
                ModelState.AddModelError("Email", errorMessage: "The Email should be same as signed in.");
            }
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            return View();
        }
        

    }
}
