namespace OnlineShop.Tests.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.AspNetCore.Hosting;

    using Moq;

    using OnlineShop.Areas.Admin.Models.Products;
    using OnlineShop.Data.Models;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;
    using OnlineShop.Tests.Mocks;

    public class ProductServiceTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        public ProductServiceTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        }

        [Fact]
        public async Task AddAsyncShouldAddEntity()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.SaveChanges();

            var model = new ProductFormModel()
            {
                Name = "New Product",
                Description = "New Product description",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            };

            var productService = new ProductService(dbContext, _categoryServiceMock.Object, _webHostEnvironmentMock.Object);

            //Act
            await productService.AddAsync(model, null, CancellationToken.None);

            var result = dbContext.Products.Count();

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddAsyncShouldAddEntityWhenAddedByStore()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Stores.Add(new StoreEntity() { Id = 1, Name = "Test Store1", AdditionalInfo = "Test Store1 additionalInfo", Description = "Test Store1 description" });
            dbContext.SaveChanges();

            var model = new ProductFormModel()
            {
                Name = "New Product",
                Description = "New Product description",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            };

            var productService = new ProductService(dbContext, _categoryServiceMock.Object, _webHostEnvironmentMock.Object);

            //Act
            await productService.AddAsync(model, 1, CancellationToken.None);

            var result = await productService.GetByIdAsync(1, CancellationToken.None);

            //Assert
            result.Should()
                .NotBeNull()
                .And.BeAssignableTo<ProductDto>();

            result.StoreId.Should().Be(1);
        }

        [Fact]
        public async Task EditAsyncShouldUpdateProduct()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 2, Name = "Category2" });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 3,
                Name = "Product",
                Description = "Product description",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            });
            dbContext.SaveChanges();

            var model = new ProductFormModel()
            {
                Name = "Edited Product",
                Description = "Edited Product description",
                Price = 5.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 2,
            };

            var productService = new ProductService(dbContext, _categoryServiceMock.Object, _webHostEnvironmentMock.Object);

            //Act
            var result = await productService.EditAsync(3, model, CancellationToken.None);

            var edited = await productService.GetByIdAsync(3, CancellationToken.None);

            //Assert
            Assert.Equal(3, result);

            edited.Name.Should().Be(model.Name);
            edited.Price.Should().Be(model.Price);
            edited.Description.Should().Be(model.Description);
            edited.CategoryId.Should().Be(model.CategoryId);
        }

        [Fact]
        public async Task EditAsyncShouldReturnsNullWhenProductNotExist()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;

            var model = new ProductFormModel()
            {
                Name = "Edited Product",
                Description = "Edited Product description",
                Price = 5.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 2,
            };

            var productService = new ProductService(dbContext, _categoryServiceMock.Object, _webHostEnvironmentMock.Object);

            //Act
            var result = await productService.EditAsync(-1, model, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdShouldReturnDto()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 2, Name = "Category2" });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 3,
                Name = "Product",
                Description = "Product description",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            });
            dbContext.SaveChanges();

            var expectedResult = new ProductDto()
            {
                Id = 3,
                Name = "Product",
                Description = "Product description",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
                Category = "Category1",
            };

            var productService = new ProductService(dbContext, _categoryServiceMock.Object, _webHostEnvironmentMock.Object);

            //Act
            var result = await productService.GetByIdAsync(3, CancellationToken.None);


            //Assert
            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdShouldReturnNullIfProductNotExist()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;

            var productService = new ProductService(dbContext, _categoryServiceMock.Object, _webHostEnvironmentMock.Object);

            //Act
            var result = await productService.GetByIdAsync(-1, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsyncShouldRemoveProductEntity()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 2, Name = "Category2" });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 3,
                Name = "Product",
                Description = "Product description",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 4,
                Name = "Product4",
                Description = "Product description4",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl4",
                CategoryId = 2,
            });
            dbContext.SaveChanges();


            var productService = new ProductService(dbContext, _categoryServiceMock.Object, _webHostEnvironmentMock.Object);

            //Act
            await productService.DeleteAsync(4, CancellationToken.None);

            //Assert
            Assert.Equal(1, dbContext.Products.Count());
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllProducts()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 2, Name = "Category2" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 3, Name = "Category3" });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 1,
                Name = "T-shirt",
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 2,
                Name = "Hair Mask",
                Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.",
                Price = 15.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 2,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 3,
                Name = "Car",
                Description = "Motor vehicle with wheels.",
                Price = 5000.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 3,
            });
            dbContext.SaveChanges();

            var model = new AllProductsDto()
            {
                PageIndex = 1,
                PageSize = 2,
                SearchTerm = "",
                Sorting = Models.Products.Sorting.Unknown,
                CategoryId = null,
            };

            var categoryService = new CategoryService(dbContext);
            var productService = new ProductService(dbContext, categoryService, _webHostEnvironmentMock.Object);

            var expectedProductsResult = new List<ProductDto>()
            {
                new ProductDto()
                {
                    Id = 3,
                    Name = "Car",
                    Description = "Motor vehicle with wheels.",
                    Price = 5000.00m,
                    ImageUrl = "/test/imageUrl",
                    CategoryId = 3,
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

            //Act
            var result = await productService.GetAllAsync(model, CancellationToken.None);


            //Assert
            result.Products.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(expectedProductsResult)
                .And
                .HaveCount(2);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnProductWithSeachTerm()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 2, Name = "Category2" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 3, Name = "Category3" });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 1,
                Name = "T-shirt",
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 2,
                Name = "Hair Mask",
                Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.",
                Price = 15.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 2,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 3,
                Name = "Car",
                Description = "Motor vehicle with wheels.",
                Price = 5000.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 3,
            });
            dbContext.SaveChanges();

            var model = new AllProductsDto()
            {
                PageIndex = 1,
                PageSize = 2,
                SearchTerm = "Mask",
                Sorting = Models.Products.Sorting.Unknown,
                CategoryId = null,
            };

            var categoryService = new CategoryService(dbContext);
            var productService = new ProductService(dbContext, categoryService, _webHostEnvironmentMock.Object);

            var expectedProductsResult = new List<ProductDto>()
            {
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

            //Act
            var result = await productService.GetAllAsync(model, CancellationToken.None);


            //Assert
            result.Products.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(expectedProductsResult)
                .And
                .HaveCount(1);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnProductWithSorting()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 2, Name = "Category2" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 3, Name = "Category3" });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 1,
                Name = "T-shirt",
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 2,
                Name = "Hair Mask",
                Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.",
                Price = 15.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 2,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 3,
                Name = "Car",
                Description = "Motor vehicle with wheels.",
                Price = 5000.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 3,
            });
            dbContext.SaveChanges();

            var model = new AllProductsDto()
            {
                PageIndex = 1,
                PageSize = 2,
                SearchTerm = "",
                Sorting = Models.Products.Sorting.Price,
                CategoryId = null,
            };

            var categoryService = new CategoryService(dbContext);
            var productService = new ProductService(dbContext, categoryService, _webHostEnvironmentMock.Object);

            var expectedProductsResult = new List<ProductDto>()
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
                },
            };

            //Act
            var result = await productService.GetAllAsync(model, CancellationToken.None);

            //Assert
            result.Products.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(expectedProductsResult)
                .And
                .HaveCount(2);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnProductsFromSelectedCategory()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 2, Name = "Category2" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 3, Name = "Category3" });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 1,
                Name = "T-shirt",
                Description = "Comfortable and suitable",
                Price = 10.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 2,
                Name = "Hair Mask",
                Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.",
                Price = 15.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 1,
            });
            dbContext.Products.Add(new ProductEntity()
            {
                Id = 3,
                Name = "Car",
                Description = "Motor vehicle with wheels.",
                Price = 5000.00m,
                ImageUrl = "/test/imageUrl",
                CategoryId = 3,
            });
            dbContext.SaveChanges();

            var model = new AllProductsDto()
            {
                PageIndex = 1,
                PageSize = 2,
                SearchTerm = "",
                Sorting = Models.Products.Sorting.Name,
                CategoryId = 1,
            };

            var categoryService = new CategoryService(dbContext);
            var productService = new ProductService(dbContext, categoryService, _webHostEnvironmentMock.Object);

            var expectedProductsResult = new List<ProductDto>()
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
                    CategoryId = 1,
                }
            };

            //Act
            var result = await productService.GetAllAsync(model, CancellationToken.None);


            //Assert
            result.Products.Should()
                .NotBeEmpty()
                .And
                .BeEquivalentTo(expectedProductsResult)
                .And
                .HaveCount(2);
        }
    }
}
