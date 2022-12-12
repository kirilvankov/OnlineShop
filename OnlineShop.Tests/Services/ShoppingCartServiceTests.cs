namespace OnlineShop.Tests.Services
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;

    using Moq;

    using OnlineShop.Data;
    using OnlineShop.Data.Models;
    using OnlineShop.Exceptions;
    using OnlineShop.Models.Cart;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;
    using OnlineShop.Tests.Helpers;
    using OnlineShop.Tests.Mocks;

    public class ShoppingCartServiceTests
    {


        [Fact]
        public void AddItemShouldAddProductToCart()
        {
            //Arrange
            var dbContext = CreateDbContext();

            var shoppingCartStorageMock = new Mock<IShoppingCartStorage>();
            shoppingCartStorageMock.Setup(x => x.Retrieve()).Returns(new List<ShoppingCartStoredItem>());
            var shoppingCartService = new ShoppingCartService(dbContext, shoppingCartStorageMock.Object);

            //Act
            var result = shoppingCartService.AddItem(1);

            //Assert
            result.StoredItems.Should()
                .NotBeEmpty()
                .And
                .BeOfType<List<ShoppingCartStoredItem>>()
                .And
                .HaveCount(1);
        }

        [Fact]
        public void AddItemShouldThrowsExceptionWhenProductNotExist()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            
            var shoppingCartStorageMock = new Mock<IShoppingCartStorage>();
            shoppingCartStorageMock.Setup(x => x.Retrieve()).Returns(new List<ShoppingCartStoredItem>());

            var shoppingCartService = new ShoppingCartService(dbContext, shoppingCartStorageMock.Object);

            //Act
            //Assert
            Assert.Throws<ItemNotFoundException>(() => shoppingCartService.AddItem(-1));
        }

        [Fact]
        public void AddItemShouldIncreaseQuantityWhenProductIsAlreadyAdded()
        {
            //Arrange
            var dbContext = CreateDbContext();

            var shoppingCartStorageMock = new Mock<IShoppingCartStorage>();
            shoppingCartStorageMock.Setup(x => x.Retrieve()).Returns(new List<ShoppingCartStoredItem>());
            var shoppingCartService = new ShoppingCartService(dbContext, shoppingCartStorageMock.Object);
            var firstAdded = shoppingCartService.AddItem(1);

            //Act
            var result = shoppingCartService.AddItem(1);

            int actual = result.StoredItems.Where(x => x.ProductId == 1).Select(x => x.Quantity).First();

            //Assert
            Assert.Equal(2, actual);
        }

        [Fact]
        public void RemoveItemShouldRemoveItemFromStorage()
        {
            //Arrange
            var dbContext = CreateDbContext();

            var shoppingCartStorageMock = new Mock<IShoppingCartStorage>();
            shoppingCartStorageMock.Setup(x => x.Retrieve()).Returns(new List<ShoppingCartStoredItem>());
            var shoppingCartService = new ShoppingCartService(dbContext, shoppingCartStorageMock.Object);

            var firstAdded = shoppingCartService.AddItem(1);
            var secondAdded = shoppingCartService.AddItem(2);

            var expectedResult = new List<ShoppingCartStoredItem>()
            {
                new ShoppingCartStoredItem
                {
                    ProductId = 2,
                    Price = 15.00m,
                    Name = "Hair Mask",
                    ImageItem = "/test/imageUrl",
                    Quantity = 1
                },
            };
            //Act
            var result = shoppingCartService.RemoveItem(1);

            //Assert
            result.StoredItems.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void DecreaseShouldDecreaseQuantityWhenProductQuantityIsMoreThanOne()
        {
            //Arrange
            var dbContext = CreateDbContext();

            var shoppingCartStorageMock = new Mock<IShoppingCartStorage>();
            shoppingCartStorageMock.Setup(x => x.Retrieve()).Returns(new List<ShoppingCartStoredItem>());
            var shoppingCartService = new ShoppingCartService(dbContext, shoppingCartStorageMock.Object);

            var firstAdded = shoppingCartService.AddItem(1);
            var secondAdded = shoppingCartService.AddItem(1);

            //Act
            var result = shoppingCartService.Decrease(1);

            int actual = result.StoredItems.Where(x => x.ProductId == 1).Select(x => x.Quantity).First();

            //Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        public void ClearCartShouldClearStorage()
        {
            //Arrange
            var dbContext = CreateDbContext();                

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var testSession = new TestSession();
            mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(testSession);

            var storage = new ShoppingCartStorage(mockHttpContextAccessor.Object);

            var shoppingCartService = new ShoppingCartService(dbContext, storage);
            var firstAdded = shoppingCartService.AddItem(1);
            var secondAdded = shoppingCartService.AddItem(2);

            //Act
            shoppingCartService.ClearCart();
            var result = shoppingCartService.GetCurrentCart();

            //Assert
            result.StoredItems.Should()
                .BeEmpty()
                .And
                .HaveCount(0);
        }

        [Fact]
        public void SetOrderAddressShouldStoreTempAddress()
        {
            //Arrange
            var dbContext = CreateDbContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var testSession = new TestSession();
            mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(testSession);

            var storage = new ShoppingCartStorage(mockHttpContextAccessor.Object);

            var shoppingCartService = new ShoppingCartService(dbContext, storage);

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

            //Act
            shoppingCartService.SetOrderAddress(address);
            var result = shoppingCartService.GetOrderAddress();

            //Assert
            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(address);
        }

        private ApplicationDbContext CreateDbContext()
        {
            var dbContext = DbContextMock.Instance;
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 1,
                Name = "T-shirt",
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
                StoreId = 1,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 2,
                Name = "Hair Mask",
                Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.",
                Price = 15.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 2,
                StoreId = 1,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 3,
                Name = "Car",
                Description = "Motor vehicle with wheels.",
                Price = 5000.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 3,
                StoreId = 1,
            });
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
