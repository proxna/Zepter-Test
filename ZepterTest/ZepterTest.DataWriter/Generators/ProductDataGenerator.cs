using Bogus;
using Microsoft.Extensions.Logging;
using ZepterTest.Common;
using ZepterTest.Common.Models;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Interfaces;

namespace ZepterTest.DataWriter.Generators
{
    /// <summary>
    /// Generates mock data for products
    /// </summary>
    public class ProductDataGenerator : IDataGenerator
    {
        private readonly ZepterTestContext _context;
        private readonly DataGeneratorConfig _config;
        private readonly ILogger<ProductDataGenerator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDataGenerator"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="config">The data generator configuration</param>
        /// <param name="logger">The logger</param>
        public ProductDataGenerator(ZepterTestContext context, DataGeneratorConfig config, ILogger<ProductDataGenerator> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task GenerateDataAsync()
        {
            _logger.LogInformation("Generating {Count} products", _config.ProductCount);

            var faker = new Faker<Product>()
                .RuleFor(p => p.ProductCode, _ => Guid.NewGuid())
                .RuleFor(p => p.Price, f => Math.Round(f.Random.Decimal(10, 1000), 2))
                .RuleFor(p => p.Vat, f => Math.Round(f.Random.Decimal(0, 0.23m), 2));

            var products = faker.Generate(_config.ProductCount);

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully generated {Count} products", products.Count);
        }
    }
}
