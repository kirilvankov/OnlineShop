namespace Exam_Project.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Exam_Project.Data;
    using Exam_Project.Exceptions;
    using Exam_Project.Models.Cart;
    using Exam_Project.Services.Models;

    public class ShoppingCartService : IShopppingCartService
    {
        private readonly ProjectDbContext data;
        private readonly IShoppingCartStorage storage;
        

        public ShoppingCartService(ProjectDbContext data, IShoppingCartStorage storage)
        {
            this.data = data;
            this.storage = storage;
        }

        public CartServiceModel AddItem(int id)
        {
            var itemsInCart = storage.Retrieve();
            var existingItemInCart = itemsInCart.SingleOrDefault(s => s.ProductId == id);
            if (existingItemInCart != null)
            {
                existingItemInCart.Quantity++;
            }
            else
            {
                var item = this.data.Products.Find(id);
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

            storage.Store(itemsInCart);

            return new CartServiceModel
            { 
                StoredItems = itemsInCart
            };
        }

        public CartServiceModel RemoveItem(int id)
        {
            var itemsInCart = storage.Retrieve();

            var existingItemInCart = itemsInCart.SingleOrDefault(s => s.ProductId == id);

            if (existingItemInCart != null)
            {
                itemsInCart.Remove(existingItemInCart);
            }

            storage.Store(itemsInCart);

            return new CartServiceModel
            {
                StoredItems = itemsInCart
            };
        }

        public CartServiceModel Decrease(int id)
        {
            var itemsInCart = storage.Retrieve();
            var existingItemInCart = itemsInCart.SingleOrDefault(s => s.ProductId == id);

            if (existingItemInCart.Quantity > 1)
            {
                existingItemInCart.Quantity--;
            }

            storage.Store(itemsInCart);

            return new CartServiceModel
            {
                StoredItems = itemsInCart
            };
        }

        public CartServiceModel GetCurrentCart()
        {
            var itemsInCart = storage.Retrieve();

            return new CartServiceModel
            {
                StoredItems = itemsInCart
            };
        }

        
    }
}
