namespace OnlineShop.Tests.Services
{
    using OnlineShop.Services;

    public class ShortStringServiceTests
    {
        [Fact]
        public void GetShortStringShouldReturnSubstring()
        {
            //Arrange
            var shortStringService = new ShortStringService();
            string longString = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.";

            //Act
            var actual = shortStringService.GetShortString(longString);

            //Assert
            Assert.Equal(23, actual.Length);
        }

        [Fact]
        public void GetShortStringShouldReturnWholeShortString()
        {
            //Arrange
            var shortStringService = new ShortStringService();
            string shortString = "Mask prevent damage.";

            //Act
            var actual = shortStringService.GetShortString(shortString);

            //Assert
            Assert.Equal(shortString, actual);
        }
    }
}
