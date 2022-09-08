namespace OnlineShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineShop.Models.Cart;
    using OnlineShop.Services.Models;

    public interface IShopppingCartService
    {
        CartServiceModel AddItem(int id);

        CartServiceModel RemoveItem(int id);

        CartServiceModel Decrease(int id);

        CartServiceModel GetCurrentCart();
    }
}
