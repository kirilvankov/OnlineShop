namespace OnlineShop.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using OnlineShop.Tests.Helpers;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using OnlineShop.Controllers;
    using OnlineShop.Services;
    using OnlineShop.Models.Store;
    using OnlineShop.Services.Models;
    using OnlineShop.Data.Infrastructure;
    using Microsoft.AspNetCore.Http;
    using System.Security.Principal;

    public class StoreControllerTests 
    {
        [Fact]
        public async Task Apply_ReturnsAViewResult()
        {
            // Arrange
            var storeServiceMock = new Mock<IStoreService>();

            var controller = new StoreController(storeServiceMock.Object);

            // Act
            var result = await controller.Apply();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Apply_ReturnsRedirectWithSuccefulPost()
        {
            // Arrange
            var storeServiceMock = new Mock<IStoreService>();
            storeServiceMock.Setup(x => x.Apply(It.IsAny<RegisterStoreDto>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

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

            var controller = new StoreController(storeServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            var model = new RegisterStoreFormModel()
            {
                Name = "Test",
                AdditionalDetails = "additional details",
                Description = "Description",
                AddressInfo = new Models.Address.AddressInfoInputModel()
                {
                    AddressLine1 = "Address Line 1",
                    AddressLine2 = "Address Line 2",
                    PhoneNumber = "testPhoneNumber",
                    City = "testCity",
                    PostCode = "12345",
                    Email = "test@test.com",
                    LocationLat = null,
                    LocationLng = null,
                }
            };
            // Act
            var result = await controller.Apply(model, CancellationToken.None);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("ApplyResult", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Apply_ReturnsViewWithModelWhenModelStateIsNotValid()
        {
            // Arrange
            var storeServiceMock = new Mock<IStoreService>();
            storeServiceMock.Setup(x => x.Apply(It.IsAny<RegisterStoreDto>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

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

            var controller = new StoreController(storeServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            var model = new RegisterStoreFormModel()
            {
                Name = "Test",
                AdditionalDetails = "additional details",
                Description = "Description",
                AddressInfo = new Models.Address.AddressInfoInputModel()
                {
                    AddressLine1 = null,
                    AddressLine2 = "Address Line 2",
                    PhoneNumber = "testPhoneNumber",
                    City = "testCity",
                    PostCode = "12345",
                    Email = "test@test.com",
                    LocationLat = null,
                    LocationLng = null,
                }
            };
            controller.ModelState.AddModelError("AddressLine1", "Required");
            // Act
            var result = await controller.Apply(model, CancellationToken.None);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<RegisterStoreFormModel>(viewResult.ViewData.Model);
            Assert.Equal(model, viewModel);
        }
    }
}
