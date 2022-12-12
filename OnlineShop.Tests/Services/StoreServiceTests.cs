namespace OnlineShop.Tests.Services
{
	using Microsoft.AspNetCore.Identity;

	using Moq;

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

			int expected = 1;
			//Act
			var actual = await storeService.Apply(model, "FirstUserId", CancellationToken.None);

			Assert.Equal(expected, actual);
		} 

		private ApplicationDbContext CreateDbContext()
		{
			var dbContext = DbContextMock.Instance;

			dbContext.Users.Add(new ApplicationUser() { Id = "FirstUserId", FirstName = "FirstName", LastName = "LastName" });

			dbContext.SaveChanges();
			return dbContext;
		}
	}
}
