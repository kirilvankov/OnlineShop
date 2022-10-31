namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Data;
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

    }
}
