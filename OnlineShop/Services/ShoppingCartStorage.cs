namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Text.Json;

    using OnlineShop.Models.Cart;

    using Microsoft.AspNetCore.Http;
    using OnlineShop.Services.Models;

    public class ShoppingCartStorage : IShoppingCartStorage
    {
        const string SessionKey = "_ShoppingCartItems";
        const string SessionAddressKey = "_OrderDeliveryAddress";

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

        public void ClearStorage()
        {
            Session.SetString(SessionKey, string.Empty);
            Session.SetString(SessionAddressKey, string.Empty);
        }
        public void SetOrderAddress(AddressInfoDto model)
        {
            var addressAsString = JsonSerializer.Serialize(model);
            Session.SetString(SessionAddressKey, addressAsString);
        }
        public AddressInfoDto GetOrderAddress()
        {
            var addressAsString = Session.GetString(SessionAddressKey);
            return string.IsNullOrWhiteSpace(addressAsString) ? null : JsonSerializer.Deserialize<AddressInfoDto>(addressAsString);
        }
    }
}
