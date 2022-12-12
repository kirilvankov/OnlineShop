namespace OnlineShop.Tests.Services
{
    using FluentAssertions;

    using Microsoft.AspNetCore.Http;

    using Moq;

    using OnlineShop.Models.Cart;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;
    using OnlineShop.Tests.Helpers;

    public class ShoppingCartStorageTests
    {
        
        [Fact]
        public void StoreShouldAddItemsToSession()
        {
            //Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var testSession = new TestSession();
            mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(testSession);

            var storage = new ShoppingCartStorage(mockHttpContextAccessor.Object);

            var items = new List<ShoppingCartStoredItem>()
            {
                new ShoppingCartStoredItem(){ ProductId = 1, Price = 5.00m, Name="Product1", Quantity = 1 },
                new ShoppingCartStoredItem(){ ProductId = 2, Price = 8.00m, Name="Product2", Quantity = 2 },
            };

            //Act
            storage.Store(items);

            //Assert
            var result = storage.Retrieve();
            result.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(items)
                .And
                .HaveCount(2);

        }

        [Fact]
        public void ClearShouldRemoveAllStoredItems()
        {
            //Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var testSession = new TestSession();
            mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(testSession);

            var storage = new ShoppingCartStorage(mockHttpContextAccessor.Object);

            var items = new List<ShoppingCartStoredItem>()
            {
                new ShoppingCartStoredItem(){ ProductId = 1, Price = 5.00m, Name="Product1", Quantity = 1 },
                new ShoppingCartStoredItem(){ ProductId = 2, Price = 8.00m, Name="Product2", Quantity = 2 },
            };
            storage.Store(items);
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

            storage.SetOrderAddress(address);

            //Act
            storage.ClearStorage();

            //Assert
            var shoppingCartProducts = storage.Retrieve();
            var tempAddress = storage.GetOrderAddress();

            tempAddress.Should()
                .BeNull();
            shoppingCartProducts.Should()
                .BeEmpty();
        }

        [Fact]
        public void SetOrderAddressShouldStoreTempAddress()
        {
            //Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var testSession = new TestSession();
            mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(testSession);

            var storage = new ShoppingCartStorage(mockHttpContextAccessor.Object);

            var address = new AddressInfoDto
            {
               AddressLine1 = "Address Line 1",
               AddressLine2 = "Address Line 2",
               PhoneNumber = "testPhoneNumber",
               City = "testCity",
               PostCode= "12345",
               Email = "test@test.com",
               LocationLat = null,
               LocationLng = null,
            };

            //Act
            storage.SetOrderAddress(address);

            //Assert
            var addressResult = storage.GetOrderAddress();
            addressResult.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(address);
        }
    }
}
