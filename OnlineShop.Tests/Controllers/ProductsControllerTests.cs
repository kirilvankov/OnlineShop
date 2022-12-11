namespace OnlineShop.Tests.Controllers
{
    using System.Net;

    using FluentAssertions;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using OnlineShop.Controllers;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;
    using OnlineShop.Tests.Helpers;

    using Xunit;

    public class ProductsControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;

        private readonly Mock<IProductService> _productServiceMock;
        public ProductsControllerTest(CustomWebApplicationFactory<Startup> customWebApplicationFactory)
        {
            _customWebApplicationFactory = customWebApplicationFactory;
            _productServiceMock = new Mock<IProductService>();
        }

        [Fact]
        public async Task AllShouldReturnNotEmptyResponse()
        {
            //Arrange
            var client = _customWebApplicationFactory.CreateClient();
            //Act
            var response = await client.GetAsync("/Products/All");


            //Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DetailsShouldReturnViewWhenProductExist()
        {

            //Arrange
            var client = _customWebApplicationFactory.CreateClient();
            //Act
            var response = await client.GetAsync("/Products/Details/1");

            //Assert
            Assert.NotNull(response);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        //[Fact]
        //public async Task DetailsShouldReturnNotFoundIfProductNotExist()
        //{
        //    //Arrange
        //    var client = _customWebApplicationFactory.CreateClient();
        //    //Act
        //    var response = await client.GetAsync("/Products/Details/-1");

        //    var x = response.Content.ReadAsStringAsync();


        //    //Assert
        //    Assert.NotNull(response);
        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        //}
    }
}
