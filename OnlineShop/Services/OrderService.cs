namespace OnlineShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Areas.Admin.Models.Orders;
    using OnlineShop.Data;
    using OnlineShop.Data.Models;
    using OnlineShop.Models.Cart;
    using OnlineShop.Services.Models;

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<OrderDto>> GetUserOrders(string userId, CancellationToken cancellationToken)
         => await _dbContext.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDto
                {
                    CreatedAt = o.CreatedAt,
                    UserId = o.UserId,
                    Items = o.Products.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ImageUrl = oi.Product.ImageUrl,
                        ProductId = oi.ProductId,
                        ProductPrice = oi.Product.Price,
                        Name = oi.Product.Name,
                        Quantity = oi.Quantity
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

        public async Task<int> CreateOrder(CreateOrderInputModel input, CancellationToken cancellationToken)
        {
            var payment = new PaymentEntity()
            {
                TransactionId = input.Transaction.TransactionId,
                Status = input.Transaction.Status,
                CurrencyCode = input.Transaction.Amount.currency_code,
                CurrencyValue = input.Transaction.Amount.Value,
                CreatedOn = input.Transaction.create_time,
            };
            var order = new OrderEntity()
            {
                UserId = input.UserId,
                CreatedAt = DateTime.UtcNow,
                Payment = payment,
            };

            var products = input.Products.Select(p => new OrderProductEntity()
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                Order = order,
            }).ToList();

            var address = new AddressInfoEntity()
            {
                AddressLine1 = input.AddressInfo.AddressLine1,
                AddressLine2 = input.AddressInfo.AddressLine2,
                PhoneNumber = input.AddressInfo.PhoneNumber,
                City = input.AddressInfo.City,
                Email = input.AddressInfo.Email,
                PostCode = input.AddressInfo.PostCode,
                LocationLat = input.AddressInfo.LocationLat,
                LocationLng = input.AddressInfo.LocationLng,
            };

            order.Products = products;
            payment.Order = order;

            var orderAddress = new OrderAddressEntity()
            {
                Order = order,
                AddressInfo = address,
            };

            await _dbContext.OrderProducts.AddRangeAsync(products);
            await _dbContext.OrderAddresses.AddAsync(orderAddress);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
