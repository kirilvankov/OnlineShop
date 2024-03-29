﻿namespace OnlineShop.Models.User
{
    using OnlineShop.Models.Address;

    public class OrderUserViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressInfoViewModel AddressInfo { get; set; }
    }
}
