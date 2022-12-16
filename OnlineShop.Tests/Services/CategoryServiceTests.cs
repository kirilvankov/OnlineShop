namespace OnlineShop.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using OnlineShop.Data.Models;
    using OnlineShop.Services;
    using OnlineShop.Services.Models;
    using OnlineShop.Tests.Mocks;

    public class CategoryServiceTests
    {
        [Fact]
        public void CategoryExistShoudReturnTrueIfCategoryExist()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.SaveChanges();
            var categoryService = new CategoryService(dbContext);

            //Act
            var result = categoryService.CategoryExist(1);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CategoryExistShoudReturnFalseWithNonExistingCategory()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.SaveChanges();
            var categoryService = new CategoryService(dbContext);

            //Act
            var result = categoryService.CategoryExist(-1);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllCategoriesShouldReturnAllCategories()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            dbContext.Categories.Add(new CategoryEntity() { Id = 1, Name = "Category1" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 2, Name = "Category2" });
            dbContext.Categories.Add(new CategoryEntity() { Id = 3, Name = "Category3" });
            dbContext.SaveChanges();
            var categoryService = new CategoryService(dbContext);

            //Act
            List<CategoryDto> result = await categoryService.GetAllCategories(CancellationToken.None);

            //Assert
            result.Should()
                .NotBeEmpty()
                .And.HaveCount(3)
                .And.AllBeAssignableTo<CategoryDto>();
        }

        [Fact]
        public async Task GetAllCategoriesShouldReturnNull()
        {
            //Arrange
            var dbContext = DbContextMock.Instance;
            var categoryService = new CategoryService(dbContext);

            //Act
            List<CategoryDto> result = await categoryService.GetAllCategories(CancellationToken.None);

            //Assert
            result.Should()
                .BeEmpty()
                .And.HaveCount(0);
        }
    }
}
