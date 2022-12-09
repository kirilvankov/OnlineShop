﻿namespace OnlineShop.Models.Store
{
    using OnlineShop.Models.Address;

    public class StoreDetailsViewModel : StoreViewModel
    {
        public string UserId { get; set; }
        public AddressInfoViewModel AddressInfo { get; set; }
    }
}
