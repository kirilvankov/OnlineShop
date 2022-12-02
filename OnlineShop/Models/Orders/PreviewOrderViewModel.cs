namespace OnlineShop.Models.Orders
{
    using OnlineShop.Models.Address;
    using OnlineShop.Models.User;
    using OnlineShop.Services.Models;

    public class PreviewOrderViewModel
    {
        public CartServiceModel Cart { get; set; }
        public OrderUserViewModel UserDetails { get; set; }
    }
}
