using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ZepterTest.Common;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Generators;
using ZepterTest.DataWriter.Tests.Helpers;

namespace ZepterTest.DataWriter.Tests.Generators
{
    public class ClientDataGeneratorTests
    {
        [Fact]
        public async Task GenerateDataAsync_WithValidConfig_GeneratesClients()
        {
            // Arrange
            var databaseName = $"ClientDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ClientDataGenerator>>();
            var config = new DataGeneratorConfig { ClientCount = 10 };

            var generator = new ClientDataGenerator(context, config, loggerMock.Object);

            // Act
            await generator.GenerateDataAsync();

            // Assert
            var clients = context.Clients.ToList();
            Assert.Equal(10, clients.Count);
            Assert.All(clients, client => 
            {
                Assert.NotEmpty(client.Street);
                Assert.NotEmpty(client.City);
                Assert.NotEmpty(client.PostCode);
            });
        }

        [Fact]
        public async Task GenerateDataAsync_WithZeroCount_GeneratesNoClients()
        {
            // Arrange
            var databaseName = $"ClientDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ClientDataGenerator>>();
            var config = new DataGeneratorConfig { ClientCount = 0 };

            var generator = new ClientDataGenerator(context, config, loggerMock.Object);

            // Act
            await generator.GenerateDataAsync();

            // Assert
            var clients = context.Clients.ToList();
            Assert.Empty(clients);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ClientDataGenerator>>();
            var config = new DataGeneratorConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ClientDataGenerator(null!, config, loggerMock.Object));
            
            Assert.Equal("context", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullConfig_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"ClientDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var loggerMock = new Mock<ILogger<ClientDataGenerator>>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ClientDataGenerator(context, null!, loggerMock.Object));
            
            Assert.Equal("config", exception.ParamName);
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var databaseName = $"ClientDataGeneratorTests_{Guid.NewGuid()}";
            var context = TestDbContextFactory.CreateDbContext(databaseName);
            var config = new DataGeneratorConfig();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ClientDataGenerator(context, config, null!));
            
            Assert.Equal("logger", exception.ParamName);
        }
    }
}
