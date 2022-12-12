namespace OnlineShop.Tests.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using OnlineShop.Data.Models;
	using OnlineShop.Data;
	using OnlineShop.Tests.Mocks;
	using OnlineShop.Services;
	using FluentAssertions;
	using OnlineShop.Services.Models;
	using OnlineShop.Models.Cart;
	using OnlineShop.Areas.Admin.Models.Orders;

	public class OrderServiceTests
	{
		[Fact]
		public async Task GetUserOrdersShouldRetunAllOrdersPerUser()
		{
			//Arrange
			var dbContext = CreateDbContext();
			var orderService = new OrderService(dbContext);

			//Act
			var result = await orderService.GetUserOrders("FirstUserId", CancellationToken.None);

			//Assert
			result.Should()
				.NotBeEmpty()
				.And
				.HaveCount(2);

			result.Select(x => x.Items.Should()
					.BeOfType<List<OrderItemDto>>()
					.And
					.HaveCount(2));

		}

		[Fact]
		public async Task CreateOrderShouldCreateNewEntity()
		{
			//Arrange
			var dbContext = CreateDbContext();
			var orderService = new OrderService(dbContext);

			var address = new AddressInfoDto
			{
				AddressLine1 = "Address Line 1",
				AddressLine2 = "Address Line 2",
				PhoneNumber = "testPhoneNumber",
				City = "testCity",
				PostCode = "12345",
				Email = "test@test.com",
				LocationLat = null,
				LocationLng = null,
			};
			var createOrderInputModelDto = new CreateOrderInputModelDto()
			{
				UserId = "SecondUserId",
				FirstName = "FirstName",
				LastName = "LastName",
				Products = new List<ShoppingCartStoredItem>() { new ShoppingCartStoredItem() { ProductId = 2, Price = 15.00m } },
				TotalPrice = 15.00m,
				Transaction = new TransactionInputModel() { TransactionId = "transactionId", Status = "approved", Amount = new AmountInputModel() { currency_code = "EUR", Value = 15.00m } },
				AddressInfo = address,
			};

			//Act
			var result = await orderService.CreateOrder(createOrderInputModelDto, CancellationToken.None);

			//Assert
			Assert.NotEqual(0, result);
			Assert.Equal(4, result);
			
		}

		private ApplicationDbContext CreateDbContext()
		{
			var dbContext = DbContextMock.Instance;
			ICollection<ProductEntity> products = new List<ProductEntity>()
			{
				new ProductEntity()
				{
					Id = 1,
					Name = "T-shirt",
					Description = "Comfortable and suitable",
					Price = 10.00m,
					ImageUrl = "/test/imageUrl",
					CategoryId = 1,
					StoreId = 1,
				},
				new ProductEntity()
				{
					Id = 2,
					Name = "Hair Mask",
					Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.",
					Price = 15.00m,
					ImageUrl = "/test/imageUrl",
					CategoryId = 2,
					StoreId = 1,
				},
				new ProductEntity()
				{
					Id = 3,
					Name = "Car",
					Description = "Motor vehicle with wheels.",
					Price = 5000.00m,
					ImageUrl = "/test/imageUrl",
					CategoryId = 3,
					StoreId = 1,
				}
			};


			dbContext.Products.AddRange(products);

			dbContext.Users.Add(new ApplicationUser() { Id = "FirstUserId", FirstName = "FirstName", LastName = "LastName" });
			dbContext.Users.Add(new ApplicationUser() { Id = "SecondUserId", FirstName = "FirstName", LastName = "LastName" });

			dbContext.OrderProducts.AddRange(new List<OrderProductEntity>() { new OrderProductEntity() { OrderId = 1, ProductId = 1 }, new OrderProductEntity() { OrderId = 1, ProductId = 2 } });
			dbContext.OrderProducts.AddRange(new List<OrderProductEntity>() { new OrderProductEntity() { OrderId = 2, ProductId = 1 }, new OrderProductEntity() { OrderId = 2, ProductId = 3 } });
			dbContext.OrderProducts.AddRange(new List<OrderProductEntity>() { new OrderProductEntity() { OrderId = 3, ProductId = 2 }, new OrderProductEntity() { OrderId = 3, ProductId = 3 } });

			dbContext.Orders.Add(new OrderEntity() { Id = 1, UserId = "FirstUserId" });
			dbContext.Orders.Add(new OrderEntity() { Id = 2, UserId = "SecondUserId" });
			dbContext.Orders.Add(new OrderEntity() { Id = 3, UserId = "FirstUserId" });
			

			dbContext.SaveChanges();

			return dbContext;
		}
	}
}
