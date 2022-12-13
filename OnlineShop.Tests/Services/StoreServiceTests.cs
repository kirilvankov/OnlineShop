namespace OnlineShop.Tests.Services
{
	using FluentAssertions;

	using OnlineShop.Data;
	using OnlineShop.Data.Models;
	using OnlineShop.Services;
	using OnlineShop.Services.Models;
	using OnlineShop.Tests.Mocks;

	public class StoreServiceTests
	{
		[Fact]
		public async Task ApplyShouldCreateEntity()
		{
			//Arrange
			var dbContext = CreateDbContext();

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

			var model = new RegisterStoreDto()
			{
				Name = "TestApply",
				Description = "Test",
				AdditionalDetails = "Additional details",
				AddressInfo = address
			};

			var storeService = new StoreService(dbContext, null,  null);

			int expected = 3;
			//Act
			var actual = await storeService.Apply(model, "FirstUserId", CancellationToken.None);

			//Assert
			Assert.Equal(expected, actual);
		}

        [Fact]
        public async Task GetByIdShouldReturnId()
        {
            //Arrange
            var dbContext = CreateDbContext();

            var storeService = new StoreService(dbContext, null, null);

            int expected = 1;
            //Act
            var actual = await storeService.GetStoreId("SecondUserId", CancellationToken.None);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByIdShouldReturnNullWhenStoreNotExist()
        {
            //Arrange
            var dbContext = CreateDbContext();

            var storeService = new StoreService(dbContext, null, null);

            //Act
            var actual = await storeService.GetStoreId("FirstUserId", CancellationToken.None);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetStoresShouldReturnAllStores()
        {
            //Arrange
            var dbContext = CreateDbContext();

            var storeService = new StoreService(dbContext, null, null);

			var expected = new List<StoreDto>()
			{
				new StoreDto(){Id = 1, Name = "TestStore1", AdditionalDetails = "TestStore1 additional info", Description = "TestStore1 description", Status = Data.Enums.Status.Pending, UserId = "SecondUserId", AddressInfo = new AddressInfoDto(){ AddressLine1 = "line1", City = "city", Email = "test@test", PostCode = "123", PhoneNumber = "12345"}},
				new StoreDto(){Id = 2, Name = "TestStore2", AdditionalDetails = "TestStore2 additional info", Description = "TestStore2 description", Status = Data.Enums.Status.Pending, UserId = "ThirdUserId", AddressInfo = new AddressInfoDto(){AddressLine1 = "line1", City = "city", Email = "test@test", PostCode = "123", PhoneNumber = "12345"}}
			};

            //Act
            var actual = await storeService.GetStores(CancellationToken.None);

			//Assert
			actual.Should()
				.NotBeEmpty()
				.And
				.BeEquivalentTo(expected);
        }

        private ApplicationDbContext CreateDbContext()
		{
			var dbContext = DbContextMock.Instance;

			dbContext.Users.Add(new ApplicationUser() { Id = "FirstUserId", FirstName = "FirstName", LastName = "LastName" });
			dbContext.Users.Add(new ApplicationUser() { Id = "SecondUserId", FirstName = "SecondName", LastName = "SecondName" });
			dbContext.Users.Add(new ApplicationUser() { Id = "ThirdUserId", FirstName = "ThirdName", LastName = "ThirdName" });

			dbContext.Stores.Add(new StoreEntity() { Id = 1, Name = "TestStore1", AdditionalInfo = "TestStore1 additional info", Description = "TestStore1 description", Status = Data.Enums.Status.Pending, UserId = "SecondUserId" });
			dbContext.Stores.Add(new StoreEntity() { Id = 2, Name = "TestStore2", AdditionalInfo = "TestStore2 additional info", Description = "TestStore2 description", Status = Data.Enums.Status.Pending, UserId = "ThirdUserId" });

			dbContext.AddressInfo.Add(new AddressInfoEntity() { Id = 1, AddressLine1 = "line1", City = "city", Email = "test@test", PostCode = "123", PhoneNumber = "12345", StoreId = 1 });
			dbContext.AddressInfo.Add(new AddressInfoEntity() { Id = 2, AddressLine1 = "line1", City = "city", Email = "test@test", PostCode = "123", PhoneNumber = "12345", StoreId = 2 });
			
			dbContext.SaveChanges();

			return dbContext;
		}
	}
}
