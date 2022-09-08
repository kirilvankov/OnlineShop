namespace OnlineShop.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Data;
    using OnlineShop.Data.Infrastructure;
    using OnlineShop.Models.Orders;
    using OnlineShop.Services;

    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IOrderService _orderService;

        public OrdersController(ApplicationDbContext data, IOrderService orderService)
        {
            this.data = data;
            _orderService = orderService;
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
        public async Task<IActionResult> MyOrders(CancellationToken cancellationToken)
        {
            var userId = this.User.GetId();

            var res = await _orderService.GetUserOrders(userId, cancellationToken);

            var result = new AllOrdersViewModel();
            result.Orders = res.Select(o => new OrderViewModel
            {
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(oi => new OrderItemsViewModel
                {
                    Id = oi.Id,
                    Name = oi.Name,
                    ProductId = oi.ProductId,
                    ProductPrice = oi.ProductPrice,
                    ImageUrl = oi.ImageUrl,
                    Quantity = oi.Quantity
                }).ToList(),
                TotalPrice = o.Items.Sum(oi => oi.ProductPrice)
            }).ToList();
            
            return View(result);
        }
    }
}
