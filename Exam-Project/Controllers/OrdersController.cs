namespace Exam_Project.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Exam_Project.Data;
    using Exam_Project.Data.Infrastructure;
    using Exam_Project.Data.Models;
    using Exam_Project.Models.Orders;

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
            var userId = this.User.GetId();
            var product = this.data.Products.Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            // = this.data.Users.Where(u => u.Id == userId && u.Orders.Any(o => o.CreatedAt.Date == DateTime.UtcNow.Date)).Select(u=>u.Orders.Select(o=>o.Id)).FirstOrDefault();
            var userOrder = this.data.Orders.Where(o => o.UserId == userId && o.CreatedAt.Date == DateTime.UtcNow.Date).Select(o => o.Id).FirstOrDefault();
            if (userOrder == 0)
            {
                var order = new Order
                {
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId
                };

                this.data.Orders.Add(order);
                this.data.SaveChanges();

                userOrder = order.Id;
            }
            

            var orderedItem = new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductPrice = product.Price,
                OrderId = userOrder,
                
            };

            this.data.OrderItems.Add(orderedItem);
            this.data.SaveChanges();

           
            return RedirectToAction(nameof(Cart));
        }


        [Authorize]
        public IActionResult Cart()
        {
            var userId = this.User.GetId();
            var orderId = this.data.Orders.Where(o => o.UserId == userId && o.CreatedAt.Date == DateTime.UtcNow.Date).Select(o => o.Id).FirstOrDefault();
            if (orderId == 0)
            {
                return BadRequest();
            }
            var order = this.data.Orders.Find(orderId);

            var items = this.data.OrderItems.Where(oi => oi.OrderId == orderId).Select(oi => new OrderItemsViewModel
            {
                Id = oi.Id,
                ImageUrl = oi.Product.ImageUrl,
                Name = oi.ProductName,
                ProductPrice = oi.ProductPrice,
                Quantity = oi.Quantity
            }).ToList();

            var totalPrice = items.Sum(i => i.ProductPrice);

            return View(new OrderViewModel 
            {
                Id = orderId,
                CreatedAt = order.CreatedAt,
                Items = items,
                TotalPrice = totalPrice
            });

        }

        [Authorize]
        public IActionResult Mine()
        {
            var userId = this.User.GetId();
            var orders = this.data.Orders.Where(o => o.UserId == userId);
            return null;
        }
    }
}
