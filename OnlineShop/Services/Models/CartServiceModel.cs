namespace OnlineShop.Services.Models
{
    using System.Collections.Generic;
    using System.Linq;

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
