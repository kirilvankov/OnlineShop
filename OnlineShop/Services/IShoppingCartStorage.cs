namespace OnlineShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineShop.Models.Cart;
    using OnlineShop.Services.Models;

    public interface IShoppingCartStorage
    {
        List<ShoppingCartStoredItem> Retrieve();

        void Store(List<ShoppingCartStoredItem> items);
        void ClearStorage();
        void SetOrderAddress(AddressInfoDto model);
        AddressInfoDto GetOrderAddress();
    }
}
