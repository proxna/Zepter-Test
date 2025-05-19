using Bogus;
using Microsoft.Extensions.Logging;
using ZepterTest.Common;
using ZepterTest.Common.Models;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Interfaces;

namespace ZepterTest.DataWriter.Generators
{
    /// <summary>
    /// Generates mock data for shops
    /// </summary>
    public class ShopDataGenerator : IDataGenerator
    {
        private readonly ZepterTestContext _context;
        private readonly DataGeneratorConfig _config;
        private readonly ILogger<ShopDataGenerator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopDataGenerator"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="config">The data generator configuration</param>
        /// <param name="logger">The logger</param>
        public ShopDataGenerator(ZepterTestContext context, DataGeneratorConfig config, ILogger<ShopDataGenerator> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task GenerateDataAsync()
        {
            _logger.LogInformation("Generating {Count} shops", _config.ShopCount);

            var faker = new Faker<Shop>()
                .RuleFor(s => s.Name, f => f.Company.CompanyName());

            var shops = faker.Generate(_config.ShopCount);

            await _context.Shops.AddRangeAsync(shops);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully generated {Count} shops", shops.Count);
        }
    }
}
