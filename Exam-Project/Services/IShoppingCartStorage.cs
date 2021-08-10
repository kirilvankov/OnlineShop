namespace Exam_Project.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Exam_Project.Models.Cart;

    public interface IShoppingCartStorage
    {
        List<ShoppingCartStoredItem> Retrieve();

        void Store(List<ShoppingCartStoredItem> items);
    }
}
