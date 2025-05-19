using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ZepterTest.Common;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Generators;
using ZepterTest.DataWriter.Tests.Helpers;

namespace ZepterTest.DataWriter.Tests.Generators
{
    public class ProductDataGeneratorTests
    {
        [Fact]
        public async Task GenerateDataAsync_WithValidConfig_GeneratesProducts()
        {
            // Arrange
            var databaseName = $"ProductDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ProductDataGenerator>>();
            var config = new DataGeneratorConfig { ProductCount = 5 };

            var generator = new ProductDataGenerator(context, config, loggerMock.Object);

            // Act
            await generator.GenerateDataAsync();

            // Assert
            var products = context.Products.ToList();
            Assert.Equal(5, products.Count);
            Assert.All(products, product => 
            {
                Assert.NotEqual(Guid.Empty, product.ProductCode);
                Assert.True(product.Price > 0);
                Assert.True(product.Vat >= 0);
            });
        }

        [Fact]
        public async Task GenerateDataAsync_WithZeroCount_GeneratesNoProducts()
        {
            // Arrange
            var databaseName = $"ProductDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ProductDataGenerator>>();
            var config = new DataGeneratorConfig { ProductCount = 0 };

            var generator = new ProductDataGenerator(context, config, loggerMock.Object);

            // Act
            await generator.GenerateDataAsync();

            // Assert
            var products = context.Products.ToList();
            Assert.Empty(products);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ProductDataGenerator>>();
            var config = new DataGeneratorConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ProductDataGenerator(null!, config, loggerMock.Object));
            
            Assert.Equal("context", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullConfig_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"ProductDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ProductDataGenerator>>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ProductDataGenerator(context, null!, loggerMock.Object));
            
            Assert.Equal("config", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"ProductDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var config = new DataGeneratorConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ProductDataGenerator(context, config, null!));
            
            Assert.Equal("logger", exception.ParamName);
        }
    }
}
