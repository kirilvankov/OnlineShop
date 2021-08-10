namespace Exam_Project.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Exam_Project.Models.Cart;
    using Exam_Project.Services.Models;

    public interface IShopppingCartService
    {
        CartServiceModel AddItem(int id);

        CartServiceModel RemoveItem(int id);

        CartServiceModel Decrease(int id);

        CartServiceModel GetCurrentCart();
    }
}
