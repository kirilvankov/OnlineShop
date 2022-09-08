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
        private readonly ApplicationDbContext data;
        public OrdersController(ApplicationDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Add(int id)
        {
            
            //var product = this.data.Products.Where(p => p.Id == id).FirstOrDefault();
            //if (product == null)
            //{
            //    return NotFound();
            //}
            
            //var orderId = this.data.Orders.Where(o => o.UserId == userId && o.CreatedAt.Date == DateTime.UtcNow.Date).Select(o => o.Id).FirstOrDefault();
            //OrderItem orderedItem;

            //if (orderId == 0)
            //{
            //    orderedItem = new OrderItem
            //    {
            //        ProductId = product.Id,
            //        ProductName = product.Name,
            //        ProductPrice = product.Price,
            //        Order = new Order
            //        {
            //            CreatedAt = DateTime.UtcNow,
            //            UserId = userId,

            //        },

            //    };
            //}
            //else
            //{
            //    orderedItem = new OrderItem
            //    {
            //        ProductId = product.Id,
            //        ProductName = product.Name,
            //        ProductPrice = product.Price,
            //        OrderId = orderId,

            //    };
            //}
            

            

            //this.data.OrderItems.Add(orderedItem);
            //this.data.SaveChanges();

           
            return RedirectToAction(nameof(Cart));
        }


        [Authorize]
        public IActionResult Cart()
        {
            var userId = this.User.GetId();
            var orderId = this.data.Orders.Where(o => o.UserId == userId && o.CreatedAt.Date == DateTime.UtcNow.Date).Select(o => o.Id).FirstOrDefault();
            if (orderId == 0)
            {
                return Ok("Your cart is empty.");
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
            var orders = this.data.Orders.Where(o => o.UserId == userId).Select(o => o.Id).ToList();

            
            var allOrders = new List<OrderViewModel>();
            foreach (var orderId in orders)
            {
                var items = this.data.OrderItems.Where(oi => oi.OrderId == orderId).Select(oi => new OrderItemsViewModel
                {
                    Id = oi.Id,
                    ImageUrl = oi.Product.ImageUrl,
                    Name = oi.ProductName,
                    ProductId = oi.ProductId,
                    ProductPrice = oi.ProductPrice,
                    Quantity = oi.Quantity
                }).ToList();

                var totalPrice = items.Sum(i => i.ProductPrice);
                var date = this.data.Orders.Where(o => o.Id == orderId).Select(o => o.CreatedAt).FirstOrDefault();
                var order = new OrderViewModel
                {
                    CreatedAt = date,
                    Items = items,
                    TotalPrice = totalPrice
                };
                allOrders.Add(order);
            }


            
            return View( new AllOrdersViewModel 
            { 
                Orders = allOrders
            });
        }
    }
}
