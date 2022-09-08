namespace OnlineShop.Models.Cart
{

    public class ShoppingCartStoredItem
    {
        public int ProductId { get; set; }

        public string ImageItem { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Quantity * Price;
            }
        }
    }
}
