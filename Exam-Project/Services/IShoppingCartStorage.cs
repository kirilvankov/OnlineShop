namespace OnlineShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineShop.Models.Cart;

    public interface IShoppingCartStorage
    {
        List<ShoppingCartStoredItem> Retrieve();

        void Store(List<ShoppingCartStoredItem> items);
    }
}
