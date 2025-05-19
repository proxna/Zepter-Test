using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZepterTest.Common;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Generators;
using ZepterTest.DataWriter.Interfaces;

namespace ZepterTest.DataWriter
{
    /// <summary>
    /// Main program class for the data writer application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point for the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public static async Task Main(string[] args)
        {
            Console.WriteLine("ZepterTest Data Writer - Mock Data Generator");
            Console.WriteLine("==========================================\n");

            // Configure dependency injection
            var serviceProvider = ConfigureServices();

            try
            {
                // Create DbContext directly since we're not using the Service classes for simplicity
                var context = serviceProvider.GetRequiredService<ZepterTestContext>();
                
                // Ensure the database is created
                await context.Database.EnsureCreatedAsync();
                
                // Delete existing data
                Console.WriteLine("Clearing existing data...");
                if (context.OrderProducts.Any())
                {
                    context.OrderProducts.RemoveRange(context.OrderProducts);
                    await context.SaveChangesAsync();
                }
                
                if (context.Orders.Any())
                {
                    context.Orders.RemoveRange(context.Orders);
                    await context.SaveChangesAsync();
                }
                
                if (context.Products.Any())
                {
                    context.Products.RemoveRange(context.Products);
                    await context.SaveChangesAsync();
                }
                
                if (context.Clients.Any())
                {
                    context.Clients.RemoveRange(context.Clients);
                    await context.SaveChangesAsync();
                }
                
                if (context.Shops.Any())
                {
                    context.Shops.RemoveRange(context.Shops);
                    await context.SaveChangesAsync();
                }
                
                Console.WriteLine("Data cleared successfully.");
                
                // Generate data using individual generators
                var config = serviceProvider.GetRequiredService<DataGeneratorConfig>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                
                Console.WriteLine("Starting data generation process...");
                
                // Generate shops
                var shopGenerator = new ShopDataGenerator(
                    context,
                    config,
                    loggerFactory.CreateLogger<ShopDataGenerator>());
                await shopGenerator.GenerateDataAsync();
                
                // Generate clients
                var clientGenerator = new ClientDataGenerator(
                    context,
                    config,
                    loggerFactory.CreateLogger<ClientDataGenerator>());
                await clientGenerator.GenerateDataAsync();
                
                // Generate products
                var productGenerator = new ProductDataGenerator(
                    context,
                    config,
                    loggerFactory.CreateLogger<ProductDataGenerator>());
                await productGenerator.GenerateDataAsync();
                
                // Generate orders and order products
                var orderGenerator = new OrderDataGenerator(
                    context,
                    config,
                    loggerFactory.CreateLogger<OrderDataGenerator>());
                await orderGenerator.GenerateDataAsync();
                
                Console.WriteLine("\nData generation completed successfully!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
                return;
            }
        }

        /// <summary>
        /// Configures the services for dependency injection
        /// </summary>
        /// <returns>The service provider</returns>
        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Add logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // Add configuration
            services.AddSingleton<DataGeneratorConfig>();

            // Add DbContext
            services.AddDbContext<ZepterTestContext>(options =>
            {
                // Connection string is already configured in OnConfiguring method in ZepterTestContext
            });            return services.BuildServiceProvider();
        }
    }
}
