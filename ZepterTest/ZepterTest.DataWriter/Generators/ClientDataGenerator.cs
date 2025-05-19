using Bogus;
using Microsoft.Extensions.Logging;
using ZepterTest.Common;
using ZepterTest.Common.Models;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Interfaces;

namespace ZepterTest.DataWriter.Generators
{
    /// <summary>
    /// Generates mock data for clients
    /// </summary>
    public class ClientDataGenerator : IDataGenerator
    {
        private readonly ZepterTestContext _context;
        private readonly DataGeneratorConfig _config;
        private readonly ILogger<ClientDataGenerator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDataGenerator"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="config">The data generator configuration</param>
        /// <param name="logger">The logger</param>
        public ClientDataGenerator(ZepterTestContext context, DataGeneratorConfig config, ILogger<ClientDataGenerator> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task GenerateDataAsync()
        {
            _logger.LogInformation("Generating {Count} clients", _config.ClientCount);

            var faker = new Faker<ClientInfo>()
                .RuleFor(c => c.Street, f => f.Address.StreetAddress())
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.PostCode, f => f.Address.ZipCode());

            var clients = faker.Generate(_config.ClientCount);

            await _context.Clients.AddRangeAsync(clients);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully generated {Count} clients", clients.Count);
        }
    }
}
