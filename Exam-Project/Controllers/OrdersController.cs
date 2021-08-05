namespace Exam_Project.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Exam_Project.Data;
    using Exam_Project.Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly ProjectDbContext data;
        public OrdersController(ProjectDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Add(int id)
        {
            var product = this.data.Products.Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            
            var order = new Order
            {
                CreatedAt = DateTime.UtcNow,
            };

            var orderedItem = new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductPrice = product.Price,
                OrderId = order.Id,
                
            };

            

            ;
            return Ok("order success");
        }
    }
}
