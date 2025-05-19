using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ZepterTest.Common;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Generators;
using ZepterTest.DataWriter.Tests.Helpers;

namespace ZepterTest.DataWriter.Tests.Generators
{
    public class ShopDataGeneratorTests
    {
        [Fact]
        public async Task GenerateDataAsync_WithValidConfig_GeneratesShops()
        {
            // Arrange
            var databaseName = $"ShopDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ShopDataGenerator>>();
            var config = new DataGeneratorConfig { ShopCount = 5 };

            var generator = new ShopDataGenerator(context, config, loggerMock.Object);

            // Act
            await generator.GenerateDataAsync();

            // Assert
            var shops = context.Shops.ToList();
            Assert.Equal(5, shops.Count);
            Assert.All(shops, shop => Assert.NotNull(shop.Name));
        }

        [Fact]
        public async Task GenerateDataAsync_WithZeroCount_GeneratesNoShops()
        {
            // Arrange
            var databaseName = $"ShopDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ShopDataGenerator>>();
            var config = new DataGeneratorConfig { ShopCount = 0 };

            var generator = new ShopDataGenerator(context, config, loggerMock.Object);

            // Act
            await generator.GenerateDataAsync();

            // Assert
            var shops = context.Shops.ToList();
            Assert.Empty(shops);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ShopDataGenerator>>();
            var config = new DataGeneratorConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ShopDataGenerator(null!, config, loggerMock.Object));
            
            Assert.Equal("context", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullConfig_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"ShopDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ShopDataGenerator>>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ShopDataGenerator(context, null!, loggerMock.Object));
            
            Assert.Equal("config", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"ShopDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var config = new DataGeneratorConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ShopDataGenerator(context, config, null!));
            
            Assert.Equal("logger", exception.ParamName);
        }
    }
}
