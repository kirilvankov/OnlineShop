namespace OnlineShop.Areas.Admin.Models.Orders
{
    using System;
    using System.Collections.Generic;

    using OnlineShop.Models.Address;
    using OnlineShop.Models.Cart;
    using OnlineShop.Services.Models;

    public class CreateOrderInputModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalPrice { get; set; }
        public TransactionInputModel Transaction { get; set; }
        public AddressInfoViewModel AddressInfo { get; set; }

        public List<ShoppingCartStoredItem> Products { get; set; }
    }

    

    public class TransactionInputModel
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public AmountInputModel Amount { get; set; }
        public DateTime create_time { get; set; }
    }
    public class AmountInputModel
    {
        public string currency_code { get; set; }
        public decimal Value { get; set; }
    }
}
