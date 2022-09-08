namespace OnlineShop.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;

    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly IShopppingCartService _cart;

        public CartController(IShopppingCartService cart)
        {
            _cart = cart;
        }

        public IActionResult Cart()
        {
            CartServiceModel model = _cart.GetCurrentCart();
            return View(model);
        }
        public IActionResult Add(int id)
        {
            var result = _cart.AddItem(id);

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Remove(int id)
        {
            var result = _cart.RemoveItem(id);

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Decrease(int id)
        {
            var result = _cart.Decrease(id);

            return RedirectToAction(nameof(Cart));
        }
    }
}
