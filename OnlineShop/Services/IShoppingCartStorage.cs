namespace OnlineShop.Services
{
    using System.Collections.Generic;

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
