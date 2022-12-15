namespace OnlineShop.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using OnlineShop.Areas.Admin.Models.Products;
    using OnlineShop.Controllers;
    using OnlineShop.Models.Orders;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;

    public class OrdersControllerTests
    {
        [Fact]
        public async Task MyOrdersShouldReturnAllOrdersPerUser()
        {
            //Arrange
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock.Setup(x => x.GetUserOrders(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetUserOrders()));


            var username = "testUser";
            var email = "test@test.com";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Email, email),
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(principal);

            var controller = new OrdersController(orderServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var result = await controller.MyOrders(CancellationToken.None);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<AllOrdersViewModel>(viewResult.ViewData.Model);
            viewModel.Orders.Should()
                .NotBeEmpty()
                .And
                .BeAssignableTo<IEnumerable<OrderViewModel>>()
                .And
                .HaveCount(2);
            viewModel.Orders
                .Select(x => x.Items
                                .Select(x => x).ToList()
                                            .Should()
                                            .NotBeEmpty()
                                            .And
                                            .AllBeAssignableTo<IEnumerable<OrderItemsViewModel>>()
                                            .And
                                            .HaveCount(2));

        }

        private List<OrderDto> GetUserOrders()
        {
            var orders = new List<OrderDto>()
            {
                new OrderDto()
                {
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UserId = "testUser",
                    Items = new List<OrderItemDto>()
                    {
                        new OrderItemDto()
                        {
                            ProductId = 1,
                            Name = "T-shirt",
                            ProductPrice = 10.00m,
                            ImageUrl = "/test/imageUrl",
                            Quantity= 1,
                        },
                        new OrderItemDto()
                        {
                            ProductId = 2,
                            Name = "Hair Mask",
                            ProductPrice = 15.00m,
                            ImageUrl = "/test/imageUrl",
                            Quantity= 2,
                        },
                    }
                },

                new OrderDto()
                {
                    CreatedAt = DateTime.UtcNow.AddMonths(-1),
                    UserId = "testUser",
                    Items = new List<OrderItemDto>()
                    {
                        new OrderItemDto()
                        {
                            ProductId = 1,
                            Name = "T-shirt",
                            ProductPrice = 10.00m,
                            ImageUrl = "/test/imageUrl",
                            Quantity= 1,
                        },
                        new OrderItemDto()
                        {
                            ProductId = 2,
                            Name = "Hair Mask",
                            ProductPrice = 15.00m,
                            ImageUrl = "/test/imageUrl",
                            Quantity= 2,
                        },
                    }
                }
            };

            return orders;
        }
    }
}
