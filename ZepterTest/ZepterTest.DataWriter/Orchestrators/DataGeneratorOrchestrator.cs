using Microsoft.Extensions.Logging;
using ZepterTest.DataWriter.Generators;
using ZepterTest.DataWriter.Interfaces;

namespace ZepterTest.DataWriter.Orchestrators
{
    /// <summary>
    /// Orchestrates the execution of all data generators in the correct order
    /// </summary>
    public class DataGeneratorOrchestrator : IDataGeneratorOrchestrator
    {
        private readonly IEnumerable<IDataGenerator> _generators;
        private readonly ILogger<DataGeneratorOrchestrator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorOrchestrator"/> class.
        /// </summary>
        /// <param name="shopGenerator">The shop data generator</param>
        /// <param name="clientGenerator">The client data generator</param>
        /// <param name="productGenerator">The product data generator</param>
        /// <param name="orderGenerator">The order data generator</param>
        /// <param name="logger">The logger</param>
        public DataGeneratorOrchestrator(
            ShopDataGenerator shopGenerator,
            ClientDataGenerator clientGenerator,
            ProductDataGenerator productGenerator,
            OrderDataGenerator orderGenerator,
            ILogger<DataGeneratorOrchestrator> logger)
        {
            if (shopGenerator == null) throw new ArgumentNullException(nameof(shopGenerator));
            if (clientGenerator == null) throw new ArgumentNullException(nameof(clientGenerator));
            if (productGenerator == null) throw new ArgumentNullException(nameof(productGenerator));
            if (orderGenerator == null) throw new ArgumentNullException(nameof(orderGenerator));
            
            // Define the execution order
            _generators = new List<IDataGenerator>
            {
                shopGenerator,
                clientGenerator,
                productGenerator,
                orderGenerator
            };
            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task ExecuteAllGeneratorsAsync()
        {
            _logger.LogInformation("Starting data generation process");
            
            foreach (var generator in _generators)
            {
                var generatorType = generator.GetType().Name;
                _logger.LogInformation("Executing {GeneratorType}", generatorType);
                
                try
                {
                    await generator.GenerateDataAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing {GeneratorType}: {Message}", generatorType, ex.Message);
                    throw;
                }
            }
            
            _logger.LogInformation("Data generation process completed successfully");
        }
    }
}
