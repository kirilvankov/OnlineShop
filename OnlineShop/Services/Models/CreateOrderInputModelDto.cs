namespace OnlineShop.Services.Models
{
    using System.Collections.Generic;

    using OnlineShop.Areas.Admin.Models.Orders;
    using OnlineShop.Models.Cart;

    public class CreateOrderInputModelDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalPrice { get; set; }
        public TransactionInputModel Transaction { get; set; }
        public AddressInfoDto AddressInfo { get; set; }

        public List<ShoppingCartStoredItem> Products { get; set; }
    }
}
