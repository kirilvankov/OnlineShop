namespace OnlineShop.Tests.Services
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using OnlineShop.Data;
    using OnlineShop.Data.Models;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;
    using OnlineShop.Tests.Mocks;

    public class UserServiceTests
    {
        [Theory]
        [InlineData("FirstUserId")]
        [InlineData("SecondUserId")]
        public async Task GetUserDetailsShouldRedurnUserData(string userId)
        {
            //Arrange
            var dbContext = CreateDbContext();
            var userService = new UserService(dbContext);

            //Act
            var actual = await userService.GetUserDetails(userId, CancellationToken.None);

            //Assert
            actual.Should()
                .NotBeNull()
                .And
                .BeOfType<OrderUserDetailsDto>();
            actual.Id.Should().Be(userId);

        }

        [Theory]
        [InlineData("FirstUserId")]
        [InlineData("SecondUserId")]
        public async Task SetUserAddressShouldUpdateAddressInfo(string userId)
        {
            //Arrange
            var dbContext = CreateDbContext();
            var userService = new UserService(dbContext);

            var address = new AddressInfoDto
            {
                AddressLine1 = "Address Line 1",
                AddressLine2 = "Address Line 2",
                PhoneNumber = "testPhoneNumber",
                City = "testCity",
                PostCode = "654321",
                Email = "test@test.com",
                LocationLat = null,
                LocationLng = null,
                IsPrimary = true
            };

            //Act
            userService.SetUserPrimaryAddress(address, userId);
            address.IsPrimary = false;

            //Assert
            var actual = await userService.GetUserDetails(userId, CancellationToken.None);

            Assert.NotNull(actual);
            actual.AddressInfo.Should()
                .BeEquivalentTo(address);
        }

        [Fact]
        public async Task SetUserAddressShouldAddAddressInfoToUser()
        {
            //Arrange
            string userId = "ThirdUserId";
            var dbContext = CreateDbContext();
            var userService = new UserService(dbContext);

            var address = new AddressInfoDto
            {
                AddressLine1 = "Address Line 1",
                AddressLine2 = "Address Line 2",
                PhoneNumber = "testPhoneNumber",
                City = "testCity",
                PostCode = "654321",
                Email = "test@test.com",
                LocationLat = null,
                LocationLng = null,
                IsPrimary = true
            };

            //Act
            userService.SetUserPrimaryAddress(address, userId);
            address.IsPrimary = false;

            //Assert
            var actual = await userService.GetUserDetails(userId, CancellationToken.None);

            actual.AddressInfo.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(address);
        }

        private ApplicationDbContext CreateDbContext()
        {
            var dbContext = DbContextMock.Instance;

            dbContext.Users.Add(new ApplicationUser() { Id = "FirstUserId", FirstName = "FirstName", LastName = "LastName" });
            dbContext.Users.Add(new ApplicationUser() { Id = "SecondUserId", FirstName = "SecondName", LastName = "SecondName" });
            dbContext.Users.Add(new ApplicationUser() { Id = "ThirdUserId", FirstName = "ThirdName", LastName = "ThirdName" });

            dbContext.AddressInfo.Add(new AddressInfoEntity() { Id = 1, AddressLine1 = "line1", City = "city", Email = "test@test", PostCode = "123", PhoneNumber = "12345", UserId = "FirstUserId" });
            dbContext.AddressInfo.Add(new AddressInfoEntity() { Id = 2, AddressLine1 = "line1", City = "city", Email = "test@test", PostCode = "123", PhoneNumber = "12345", UserId = "SecondUserId" });

            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
