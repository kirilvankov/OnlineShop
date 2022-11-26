namespace OnlineShop.Models.Store
{
    using OnlineShop.Data.Enums;
    using OnlineShop.Services.Models;

    public class StoreDetailsViewModel : StoreViewModel
    {
        public string UserId { get; set; }
        public AddressInfoViewModel AddressInfo { get; set; }
    }
}
