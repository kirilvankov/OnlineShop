namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Text.Json;

    using OnlineShop.Models.Cart;

    using Microsoft.AspNetCore.Http;

    public class ShoppingCartStorage : IShoppingCartStorage
    {
        const string SessionKey = "_ShoppingCartItems";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public ShoppingCartStorage(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<ShoppingCartStoredItem> Retrieve()
        {
            var cartString = Session.GetString(SessionKey);
            return string.IsNullOrWhiteSpace(cartString) ? new List<ShoppingCartStoredItem>() : JsonSerializer.Deserialize<List<ShoppingCartStoredItem>>(cartString);
        }

        public void Store(List<ShoppingCartStoredItem> items)
        {
            var cartString = JsonSerializer.Serialize(items);
            Session.SetString(SessionKey, cartString);
        }

    }
}
