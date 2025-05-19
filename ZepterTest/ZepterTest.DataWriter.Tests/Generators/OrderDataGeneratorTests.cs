using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ZepterTest.Common;
using ZepterTest.Common.Models;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Generators;
using ZepterTest.DataWriter.Tests.Helpers;

namespace ZepterTest.DataWriter.Tests.Generators
{
    public class OrderDataGeneratorTests
    {
        [Fact]
        public async Task GenerateDataAsync_WithPrerequisiteData_GeneratesOrdersAndOrderProducts()
        {
            // Arrange
            var databaseName = $"OrderDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<OrderDataGenerator>>();
            var config = new DataGeneratorConfig { OrderCount = 3, MaxProductsPerOrder = 2 };

            // Add prerequisite data
            await SeedPrerequisiteDataAsync(context);

            var generator = new OrderDataGenerator(context, config, loggerMock.Object);

            // Act
            await generator.GenerateDataAsync();

            // Assert
            var orders = await context.Orders.ToListAsync();
            var orderProducts = await context.OrderProducts.ToListAsync();
            
            Assert.Equal(3, orders.Count);
            Assert.True(orderProducts.Count > 0);
            Assert.All(orders, order => 
            {
                Assert.True(order.ClientInfoId > 0);
                Assert.True(order.ShopId > 0);
            });
        }

        [Fact]
        public async Task GenerateDataAsync_WithNoPrerequisiteData_GeneratesNoOrders()
        {
            // Arrange
            var databaseName = $"OrderDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<OrderDataGenerator>>();
            var config = new DataGeneratorConfig { OrderCount = 3 };

            var generator = new OrderDataGenerator(context, config, loggerMock.Object);

            // Act
            await generator.GenerateDataAsync();

            // Assert
            var orders = await context.Orders.ToListAsync();
            var orderProducts = await context.OrderProducts.ToListAsync();
            
            Assert.Empty(orders);
            Assert.Empty(orderProducts);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<OrderDataGenerator>>();
            var config = new DataGeneratorConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new OrderDataGenerator(null!, config, loggerMock.Object));
            
            Assert.Equal("context", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullConfig_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"OrderDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<OrderDataGenerator>>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new OrderDataGenerator(context, null!, loggerMock.Object));
            
            Assert.Equal("config", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"OrderDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var config = new DataGeneratorConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new OrderDataGenerator(context, config, null!));
            
            Assert.Equal("logger", exception.ParamName);
        }

        private async Task SeedPrerequisiteDataAsync(ZepterTestContext context)
        {
            // Add shops
            for (int i = 1; i <= 2; i++)
            {
                context.Shops.Add(new Shop { Id = i, Name = $"Test Shop {i}" });
            }
            await context.SaveChangesAsync();

            // Add clients
            for (int i = 1; i <= 2; i++)
            {
                context.Clients.Add(new ClientInfo 
                { 
                    Street = $"Street {i}",
                    City = $"City {i}",
                    PostCode = $"1000{i}"
                });
            }
            await context.SaveChangesAsync();

            // Add products
            for (int i = 1; i <= 5; i++)
            {
                context.Products.Add(new Product
                {
                    ProductCode = Guid.NewGuid(),
                    Price = 100 * i,
                    Vat = 0.1m * i
                });
            }
            await context.SaveChangesAsync();
        }
    }
}
