namespace OnlineShop.Services
{
    using OnlineShop.Services.Models;

    public interface IShopppingCartService
    {
        CartServiceModel AddItem(int id);

        CartServiceModel RemoveItem(int id);

        CartServiceModel Decrease(int id);

        CartServiceModel GetCurrentCart();

        void ClearCart();

        void SetOrderAddress(AddressInfoDto model);

        AddressInfoDto GetOrderAddress();
    }
}
