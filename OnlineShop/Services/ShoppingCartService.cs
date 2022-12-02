namespace OnlineShop.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineShop.Data;
    using OnlineShop.Exceptions;
    using OnlineShop.Models.Cart;
    using OnlineShop.Services.Models;

    public class ShoppingCartService : IShopppingCartService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IShoppingCartStorage _storage;
        

        public ShoppingCartService(ApplicationDbContext dbContext, IShoppingCartStorage storage)
        {
            _dbContext = dbContext;
            _storage = storage;
        }

        public CartServiceModel AddItem(int id)
        {
            var itemsInCart = _storage.Retrieve();
            var existingItemInCart = itemsInCart.SingleOrDefault(s => s.ProductId == id);
            if (existingItemInCart != null)
            {
                existingItemInCart.Quantity++;
            }
            else
            {
                var item = _dbContext.Products.Find(id);
                if (item == null)
                {
                    throw new ItemNotFoundException("Item not found");
                }


                itemsInCart.Add(new ShoppingCartStoredItem
                {
                    ProductId = item.Id,
                    Name = item.Name,
                    ImageItem = item.ImageUrl,
                    Price = item.Price,
                    Quantity = 1

                });

            }

            _storage.Store(itemsInCart);

            return new CartServiceModel
            { 
                StoredItems = itemsInCart
            };
        }

        public CartServiceModel RemoveItem(int id)
        {
            var itemsInCart = _storage.Retrieve();

            var existingItemInCart = itemsInCart.SingleOrDefault(s => s.ProductId == id);

            if (existingItemInCart != null)
            {
                itemsInCart.Remove(existingItemInCart);
            }

            _storage.Store(itemsInCart);

            return new CartServiceModel
            {
                StoredItems = itemsInCart
            };
        }

        public CartServiceModel Decrease(int id)
        {
            var itemsInCart = _storage.Retrieve();
            var existingItemInCart = itemsInCart.SingleOrDefault(s => s.ProductId == id);

            if (existingItemInCart.Quantity > 1)
            {
                existingItemInCart.Quantity--;
            }

            _storage.Store(itemsInCart);

            return new CartServiceModel
            {
                StoredItems = itemsInCart
            };
        }

        public CartServiceModel GetCurrentCart()
        {
            var itemsInCart = _storage.Retrieve();

            return new CartServiceModel
            {
                StoredItems = itemsInCart
            };
        }

        public void ClearCart()
        {
            _storage.ClearStorage();
        }
    }
}
