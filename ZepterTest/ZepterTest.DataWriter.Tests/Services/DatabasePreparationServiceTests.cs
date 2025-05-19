using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ZepterTest.Common;
using ZepterTest.Common.Models;
using ZepterTest.DataWriter.Services;
using ZepterTest.DataWriter.Tests.Helpers;

namespace ZepterTest.DataWriter.Tests.Services
{
    public class DatabasePreparationServiceTests
    {
        [Fact]
        public async Task PrepareDatabaseAsync_CreatesDatabase()
        {
            // Arrange
            var databaseName = $"PreparationServiceTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<DatabasePreparationService>>();
            var service = new DatabasePreparationService(context, loggerMock.Object);

            // Act
            await service.PrepareDatabaseAsync();

            // Assert - no exception means success for in-memory DB
            Assert.True(await context.Database.CanConnectAsync());
        }

        [Fact]
        public async Task ClearDatabaseAsync_RemovesAllData()
        {
            // Arrange
            var databaseName = $"ClearDatabaseTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<DatabasePreparationService>>();
            var service = new DatabasePreparationService(context, loggerMock.Object);

            // Add some test data
            await SeedTestDataAsync(context);

            // Act
            await service.ClearDatabaseAsync();

            // Assert
            Assert.Empty(await context.Shops.ToListAsync());
            Assert.Empty(await context.Clients.ToListAsync());
            Assert.Empty(await context.Products.ToListAsync());
            Assert.Empty(await context.Orders.ToListAsync());
            Assert.Empty(await context.OrderProducts.ToListAsync());
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DatabasePreparationService>>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new DatabasePreparationService(null!, loggerMock.Object));
            
            Assert.Equal("context", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"DatabasePreparationServiceTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new DatabasePreparationService(context, null!));
            
            Assert.Equal("logger", exception.ParamName);
        }

        private async Task SeedTestDataAsync(ZepterTestContext context)
        {
            // Add shop
            var shop = new Shop { Id = 1, Name = "Test Shop" };
            context.Shops.Add(shop);
            await context.SaveChangesAsync();

            // Add client
            var client = new ClientInfo 
            { 
                Street = "Test Street",
                City = "Test City",
                PostCode = "12345"
            };
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            // Add product
            var product = new Product
            {
                ProductCode = Guid.NewGuid(),
                Price = 100,
                Vat = 0.1m
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Add order
            var order = new Order
            {
                ClientInfoId = client.Id,
                ShopId = shop.Id,
                PaymentMethod = Common.Enums.PaymentMethod.Cash
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Add order product
            var orderProduct = new OrderProduct
            {
                OrderId = order.Id,
                ProductCode = product.ProductCode,
                Quantity = 2
            };
            context.OrderProducts.Add(orderProduct);
            await context.SaveChangesAsync();
        }
    }
}
