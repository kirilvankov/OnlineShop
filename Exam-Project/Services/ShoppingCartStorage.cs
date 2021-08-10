namespace Exam_Project.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Exam_Project.Models.Cart;

    using Microsoft.AspNetCore.Http;

    public class ShoppingCartStorage : IShoppingCartStorage
    {
        const string SessionKey = "_ShoppingCartItems";

        private readonly IHttpContextAccessor httpContextAccessor;

        public ShoppingCartStorage(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
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

        private ISession Session => this.httpContextAccessor.HttpContext.Session;
    }
}
