namespace Exam_Project.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exam_Project.Services;
    using Exam_Project.Services.Models;

    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly IShopppingCartService cart;

        public CartController(IShopppingCartService cart)
        {
            this.cart = cart;
        }

        public IActionResult Cart()
        {
            CartServiceModel model = cart.GetCurrentCart();
            return View(model);
        }
        public IActionResult Add(int id)
        {
            var result = cart.AddItem(id);

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Remove(int id)
        {
            var result = cart.RemoveItem(id);

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Decrease(int id)
        {
            var result = cart.Decrease(id);

            return RedirectToAction(nameof(Cart));
        }
    }
}
