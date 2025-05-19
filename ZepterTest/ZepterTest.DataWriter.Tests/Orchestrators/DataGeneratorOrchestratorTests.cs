using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ZepterTest.DataWriter.Generators;
using ZepterTest.DataWriter.Interfaces;
using ZepterTest.DataWriter.Orchestrators;

namespace ZepterTest.DataWriter.Tests.Orchestrators
{
    public class DataGeneratorOrchestratorTests
    {
        [Fact]
        public async Task ExecuteAllGeneratorsAsync_CallsAllGeneratorsInCorrectOrder()
        {
            // Arrange
            var mockShopGenerator = new Mock<ShopDataGenerator>(null!, null!, null!);
            var mockClientGenerator = new Mock<ClientDataGenerator>(null!, null!, null!);
            var mockProductGenerator = new Mock<ProductDataGenerator>(null!, null!, null!);
            var mockOrderGenerator = new Mock<OrderDataGenerator>(null!, null!, null!);
            var mockLogger = new Mock<ILogger<DataGeneratorOrchestrator>>();

            var sequence = new MockSequence();
            mockShopGenerator.InSequence(sequence).Setup(g => g.GenerateDataAsync()).Returns(Task.CompletedTask);
            mockClientGenerator.InSequence(sequence).Setup(g => g.GenerateDataAsync()).Returns(Task.CompletedTask);
            mockProductGenerator.InSequence(sequence).Setup(g => g.GenerateDataAsync()).Returns(Task.CompletedTask);
            mockOrderGenerator.InSequence(sequence).Setup(g => g.GenerateDataAsync()).Returns(Task.CompletedTask);

            var orchestrator = new DataGeneratorOrchestrator(
                mockShopGenerator.Object,
                mockClientGenerator.Object,
                mockProductGenerator.Object,
                mockOrderGenerator.Object,
                mockLogger.Object);

            // Act
            await orchestrator.ExecuteAllGeneratorsAsync();

            // Assert
            mockShopGenerator.Verify(g => g.GenerateDataAsync(), Times.Once);
            mockClientGenerator.Verify(g => g.GenerateDataAsync(), Times.Once);
            mockProductGenerator.Verify(g => g.GenerateDataAsync(), Times.Once);
            mockOrderGenerator.Verify(g => g.GenerateDataAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAllGeneratorsAsync_GeneratorThrowsException_PropagatesException()
        {
            // Arrange
            var mockShopGenerator = new Mock<ShopDataGenerator>(null!, null!, null!);
            var mockClientGenerator = new Mock<ClientDataGenerator>(null!, null!, null!);
            var mockProductGenerator = new Mock<ProductDataGenerator>(null!, null!, null!);
            var mockOrderGenerator = new Mock<OrderDataGenerator>(null!, null!, null!);
            var mockLogger = new Mock<ILogger<DataGeneratorOrchestrator>>();

            var expectedException = new InvalidOperationException("Test exception");
            mockShopGenerator.Setup(g => g.GenerateDataAsync()).Returns(Task.CompletedTask);
            mockClientGenerator.Setup(g => g.GenerateDataAsync()).ThrowsAsync(expectedException);

            var orchestrator = new DataGeneratorOrchestrator(
                mockShopGenerator.Object,
                mockClientGenerator.Object,
                mockProductGenerator.Object,
                mockOrderGenerator.Object,
                mockLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => orchestrator.ExecuteAllGeneratorsAsync());
            
            Assert.Same(expectedException, exception);
            mockShopGenerator.Verify(g => g.GenerateDataAsync(), Times.Once);
            mockClientGenerator.Verify(g => g.GenerateDataAsync(), Times.Once);
            mockProductGenerator.Verify(g => g.GenerateDataAsync(), Times.Never);
            mockOrderGenerator.Verify(g => g.GenerateDataAsync(), Times.Never);
        }

        [Fact]
        public void Constructor_NullShopGenerator_ThrowsArgumentNullException()
        {
            // Arrange
            var mockClientGenerator = new Mock<ClientDataGenerator>(null!, null!, null!);
            var mockProductGenerator = new Mock<ProductDataGenerator>(null!, null!, null!);
            var mockOrderGenerator = new Mock<OrderDataGenerator>(null!, null!, null!);
            var mockLogger = new Mock<ILogger<DataGeneratorOrchestrator>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DataGeneratorOrchestrator(
                null!,
                mockClientGenerator.Object,
                mockProductGenerator.Object,
                mockOrderGenerator.Object,
                mockLogger.Object));
        }

        [Fact]
        public void Constructor_NullClientGenerator_ThrowsArgumentNullException()
        {
            // Arrange
            var mockShopGenerator = new Mock<ShopDataGenerator>(null!, null!, null!);
            var mockProductGenerator = new Mock<ProductDataGenerator>(null!, null!, null!);
            var mockOrderGenerator = new Mock<OrderDataGenerator>(null!, null!, null!);
            var mockLogger = new Mock<ILogger<DataGeneratorOrchestrator>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DataGeneratorOrchestrator(
                mockShopGenerator.Object,
                null!,
                mockProductGenerator.Object,
                mockOrderGenerator.Object,
                mockLogger.Object));
        }

        [Fact]
        public void Constructor_NullProductGenerator_ThrowsArgumentNullException()
        {
            // Arrange
            var mockShopGenerator = new Mock<ShopDataGenerator>(null!, null!, null!);
            var mockClientGenerator = new Mock<ClientDataGenerator>(null!, null!, null!);
            var mockOrderGenerator = new Mock<OrderDataGenerator>(null!, null!, null!);
            var mockLogger = new Mock<ILogger<DataGeneratorOrchestrator>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DataGeneratorOrchestrator(
                mockShopGenerator.Object,
                mockClientGenerator.Object,
                null!,
                mockOrderGenerator.Object,
                mockLogger.Object));
        }

        [Fact]
        public void Constructor_NullOrderGenerator_ThrowsArgumentNullException()
        {
            // Arrange
            var mockShopGenerator = new Mock<ShopDataGenerator>(null!, null!, null!);
            var mockClientGenerator = new Mock<ClientDataGenerator>(null!, null!, null!);
            var mockProductGenerator = new Mock<ProductDataGenerator>(null!, null!, null!);
            var mockLogger = new Mock<ILogger<DataGeneratorOrchestrator>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DataGeneratorOrchestrator(
                mockShopGenerator.Object,
                mockClientGenerator.Object,
                mockProductGenerator.Object,
                null!,
                mockLogger.Object));
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var mockShopGenerator = new Mock<ShopDataGenerator>(null!, null!, null!);
            var mockClientGenerator = new Mock<ClientDataGenerator>(null!, null!, null!);
            var mockProductGenerator = new Mock<ProductDataGenerator>(null!, null!, null!);
            var mockOrderGenerator = new Mock<OrderDataGenerator>(null!, null!, null!);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DataGeneratorOrchestrator(
                mockShopGenerator.Object,
                mockClientGenerator.Object,
                mockProductGenerator.Object,
                mockOrderGenerator.Object,
                null!));
        }
    }
}
