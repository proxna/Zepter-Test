using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZepterTest.Common;

namespace ZepterTest.DataWriter.Services
{
    /// <summary>
    /// Service for preparing the database
    /// </summary>
    public class DatabasePreparationService
    {
        private readonly ZepterTestContext _context;
        private readonly ILogger<DatabasePreparationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabasePreparationService"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="logger">The logger</param>
        public DatabasePreparationService(ZepterTestContext context, ILogger<DatabasePreparationService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Prepares the database by creating it and applying migrations
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        public async Task PrepareDatabaseAsync()
        {
            _logger.LogInformation("Preparing database...");

            // Check if the database exists and create it if it doesn't
            await _context.Database.EnsureCreatedAsync();

            // Apply any pending migrations
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                _logger.LogInformation("Applying pending migrations");
                await _context.Database.MigrateAsync();
            }

            _logger.LogInformation("Database preparation completed");
        }

        /// <summary>
        /// Clears all data from the database
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        public async Task ClearDatabaseAsync()
        {
            _logger.LogInformation("Clearing existing data from database");

            // Clear data in reverse order of creation to avoid foreign key constraint violations
            _context.OrderProducts.RemoveRange(await _context.OrderProducts.ToListAsync());
            await _context.SaveChangesAsync();

            _context.Orders.RemoveRange(await _context.Orders.ToListAsync());
            await _context.SaveChangesAsync();

            _context.Products.RemoveRange(await _context.Products.ToListAsync());
            await _context.SaveChangesAsync();

            _context.Clients.RemoveRange(await _context.Clients.ToListAsync());
            await _context.SaveChangesAsync();

            _context.Shops.RemoveRange(await _context.Shops.ToListAsync());
            await _context.SaveChangesAsync();

            _logger.LogInformation("Database cleared successfully");
        }
    }
}
