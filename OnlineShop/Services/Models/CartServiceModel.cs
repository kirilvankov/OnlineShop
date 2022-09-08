namespace OnlineShop.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineShop.Models.Cart;

    public class CartServiceModel
    {
        public CartServiceModel()
        {
            StoredItems = new List<ShoppingCartStoredItem>();
        }

        public decimal TotalPrice => StoredItems.Sum(si => si.TotalPrice);
        public List<ShoppingCartStoredItem> StoredItems { get; set; }
    }
}
