namespace OnlineShop.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using OnlineShop.Areas.Admin.Models.Products;
    using OnlineShop.Models.Products;
    using OnlineShop.Models.Store;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;

    public class AdminProductControllerTests
    {
        [Fact]
        public async Task AllShouldReturnAllProducts()
        {
            //Arrange
            var productserviceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();
            
            productserviceMock.Setup(x => x.GetAllAsync(It.IsAny<AllProductsDto>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new AllProductsDto() { Products = GetProducts(), Categories = GetCategories()}));

            var request = new LoadProductsRequest()
            {
                PageIndex = 1,
                PageSize = 2,
            };

            var controller = new Areas.Admin.Controllers.ProductsController(productserviceMock.Object, categoryServiceMock.Object);

            //Act
            var result = await controller.All(request, CancellationToken.None);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<PagedResult<ProductViewModel>>(viewResult.ViewData.Model);
            viewModel.Products.Should()
                .NotBeEmpty()
                .And
                .BeOfType<List<ProductViewModel>>()
                .And
                .HaveCount(2);
            viewModel.Categories.Should()
                .NotBeEmpty()
                .And
                .BeOfType<List<ProductCategoriesViewModel>>()
                .And
                .HaveCount(2);
        }

        [Fact]
        public async Task AddShouldReturnReirectToAll()
        {
            //Arrange
            var productserviceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();
            
            productserviceMock.Setup(x => x.AddAsync(It.IsAny<ProductFormModel>(), It.IsAny<int?>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            categoryServiceMock.Setup(x => x.CategoryExist(It.IsAny<int>())).Returns(true);
            
            var input = new ProductFormModel()
            {
               Name = "Test",
               Description = "Description",
               Price = 10.00m,
               CategoryId = 1,
               ImageUrl = "/test/url"
            };

            var controller = new Areas.Admin.Controllers.ProductsController(productserviceMock.Object, categoryServiceMock.Object);

            //Act
            var result = await controller.Add(input, CancellationToken.None);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("All", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task AddShouldReturnViewWithModelWhenModelStateIsNotValid()
        {
            //Arrange
            var productserviceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();

            categoryServiceMock.Setup(x => x.GetAllCategories(It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetCategories()));

            var input = new ProductFormModel()
            {
                Name = null,
                Description = "Description",
                Price = 10.00m,
                CategoryId = 1,
                ImageUrl = "/test/url"
            };

            var controller = new Areas.Admin.Controllers.ProductsController(productserviceMock.Object, categoryServiceMock.Object);
            controller.ModelState.AddModelError("Name", "Required");

            //Act
            var result = await controller.Add(input, CancellationToken.None);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<ProductFormModel>(viewResult.ViewData.Model);
            Assert.Equal(input, viewModel);
        }

        [Fact]
        public async Task EditShouldReturnViewWithModel()
        {
            //Arrange
            var productserviceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();

            productserviceMock.Setup(x => x.GetByIdAsync(It.Is<int>(x => x > 0 && x < 3), It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetProducts().First(x => x.Id == 1)));
            categoryServiceMock.Setup(x => x.GetAllCategories(It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetCategories()));

            var expected = new ProductFormModel()
            {
                Name = "T-shirt",
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
                Categories = GetCategories().Select(x => new ProductCategoriesViewModel() { Id = x.Id, Name = x.Name,}).ToList(),
            };

            var controller = new Areas.Admin.Controllers.ProductsController(productserviceMock.Object, categoryServiceMock.Object);

            //Act
            var result = await controller.Edit(1, CancellationToken.None);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<ProductFormModel>(viewResult.ViewData.Model);
            viewModel.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(expected);
        }

        [Fact]
        public async Task EditShouldReturnRedirectToDetails()
        {
            //Arrange
            var productserviceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();

            productserviceMock.Setup(x => x.EditAsync(It.Is<int>(x => x > 0 && x < 3),It.IsAny<ProductFormModel>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<int?>(1));
            categoryServiceMock.Setup(x => x.GetAllCategories(It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetCategories()));

            var model = new ProductFormModel()
            {
                Name = "T-shirt",
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
                Categories = GetCategories().Select(x => new ProductCategoriesViewModel() { Id = x.Id, Name = x.Name, }).ToList(),
            };

            var controller = new Areas.Admin.Controllers.ProductsController(productserviceMock.Object, categoryServiceMock.Object);

            //Act
            var result = await controller.Edit(1, model, CancellationToken.None);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal(1, redirectToActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task EditShouldReturnViewWithModelWhenModelStateIsNotValid()
        {
            //Arrange
            var productserviceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();

            categoryServiceMock.Setup(x => x.GetAllCategories(It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetCategories()));

            var model = new ProductFormModel()
            {
                Name = null,
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
                Categories = GetCategories().Select(x => new ProductCategoriesViewModel() { Id = x.Id, Name = x.Name, }).ToList(),
            };

            var controller = new Areas.Admin.Controllers.ProductsController(productserviceMock.Object, categoryServiceMock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            //Act
            var result = await controller.Edit(1, model, CancellationToken.None);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<ProductFormModel>(viewResult.ViewData.Model);
            viewModel.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(model);
        }

        [Fact]
        public async Task EditShouldReturnRedirectToAll()
        {
            //Arrange
            var productserviceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();

            productserviceMock.Setup(x => x.EditAsync(It.Is<int>(x => x > 0 && x < 3), It.IsAny<ProductFormModel>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<int?>(null));
            categoryServiceMock.Setup(x => x.GetAllCategories(It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetCategories()));

            var model = new ProductFormModel()
            {
                Name = "T-shirt",
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
                Categories = GetCategories().Select(x => new ProductCategoriesViewModel() { Id = x.Id, Name = x.Name, }).ToList(),
            };

            var controller = new Areas.Admin.Controllers.ProductsController(productserviceMock.Object, categoryServiceMock.Object);

            //Act
            var result = await controller.Edit(-1, model, CancellationToken.None);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("All", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DeleteShouldReturnRedirectToAll()
        {
            //Arrange
            var productserviceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();

            productserviceMock.Setup(x => x.DeleteAsync(It.Is<int>(x => x > 0 && x < 3), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            
            var controller = new Areas.Admin.Controllers.ProductsController(productserviceMock.Object, categoryServiceMock.Object);

            //Act
            var result = await controller.Delete(1, CancellationToken.None);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("All", redirectToActionResult.ActionName);
        }

        private List<ProductDto> GetProducts()
        {
            var products = new List<ProductDto>()
            {
                new ProductDto()
                {
                    Id = 1,
                    Name = "T-shirt",
                    Description = "Comfortable and suitable",
                    Price = 10.00m,
                    ImageUrl = "/test/imageUrl",
                    CategoryId = 1,
                },
                new ProductDto()
                {
                    Id = 2,
                    Name = "Hair Mask",
                    Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.",
                    Price = 15.00m,
                    ImageUrl = "/test/imageUrl",
                    CategoryId = 2,
                }
            };

            return products;
        }

        private List<CategoryDto> GetCategories()
        {
            var categories = new List<CategoryDto>()
            {
                new CategoryDto() { Id = 1, Name = "Category1" },
                new CategoryDto() { Id = 2, Name = "Category2" }
            };

            return categories;
        }
    }
}
