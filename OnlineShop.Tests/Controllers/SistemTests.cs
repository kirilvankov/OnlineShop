namespace OnlineShop.Tests.Controllers
{
    using System.Net;

    using FluentAssertions;

    using Moq;

    using OnlineShop.Services;
    using OnlineShop.Tests.Helpers;

    using Xunit;

    public class SystemTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;

        private readonly Mock<IProductService> _productServiceMock;
        public SystemTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory)
        {
            _customWebApplicationFactory = customWebApplicationFactory;
            _productServiceMock = new Mock<IProductService>();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Products/All")]
        [InlineData("/Products/Details/1")]
        [InlineData("/Store/Apply")]
        public async Task AllShouldReturnNotEmptyResponse(string url)
        {
            //Arrange
            var client = _customWebApplicationFactory.CreateClient();
            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
                
        [Fact]
        public async Task DetailsShouldReturnNotFoundIfProductNotExist()
        {
            //Arrange
            var client = _customWebApplicationFactory.CreateClient();

            //Act
            var response = await client.GetAsync("/Products/Details/-1");

            //Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }
    }
}
