using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZepterTest.Common;
using ZepterTest.Common.Enums;
using ZepterTest.Common.Models;
using ZepterTest.DataWriter.Config;
using ZepterTest.DataWriter.Interfaces;

namespace ZepterTest.DataWriter.Generators
{
    /// <summary>
    /// Generates mock data for orders and order products
    /// </summary>
    public class OrderDataGenerator : IDataGenerator
    {
        private readonly ZepterTestContext _context;
        private readonly DataGeneratorConfig _config;
        private readonly ILogger<OrderDataGenerator> _logger;
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDataGenerator"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="config">The data generator configuration</param>
        /// <param name="logger">The logger</param>
        public OrderDataGenerator(ZepterTestContext context, DataGeneratorConfig config, ILogger<OrderDataGenerator> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _random = new Random();
        }

        /// <inheritdoc/>
        public async Task GenerateDataAsync()
        {
            _logger.LogInformation("Generating {Count} orders", _config.OrderCount);

            // Load all necessary data first to avoid multiple database trips
            var clients = await _context.Clients.ToListAsync();
            var shops = await _context.Shops.ToListAsync();
            var products = await _context.Products.ToListAsync();

            if (!clients.Any() || !shops.Any() || !products.Any())
            {
                _logger.LogWarning("Cannot generate orders - missing prerequisite data");
                return;
            }

            var faker = new Faker<Order>()
                .RuleFor(o => o.PaymentMethod, f => f.PickRandom<PaymentMethod>())
                .RuleFor(o => o.ClientInfoId, f => f.PickRandom(clients).Id)
                .RuleFor(o => o.ShopId, f => f.PickRandom(shops).Id);

            var orders = faker.Generate(_config.OrderCount);
            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully generated {Count} orders", orders.Count);
            
            // Now generate order products
            await GenerateOrderProductsAsync(orders, products);
        }

        /// <summary>
        /// Generates OrderProduct relationships between orders and products
        /// </summary>
        /// <param name="orders">The orders to associate with products</param>
        /// <param name="allProducts">All available products</param>
        /// <returns>Task representing the asynchronous operation</returns>
        private async Task GenerateOrderProductsAsync(List<Order> orders, List<Product> allProducts)
        {
            _logger.LogInformation("Generating order products relationships");
            
            var orderProducts = new List<OrderProduct>();
            
            foreach (var order in orders)
            {
                // Randomly select between 1 and MaxProductsPerOrder products for this order
                int productCount = _random.Next(1, Math.Min(_config.MaxProductsPerOrder + 1, allProducts.Count));
                var selectedProducts = allProducts.OrderBy(_ => _random.Next()).Take(productCount).ToList();
                
                foreach (var product in selectedProducts)
                {
                    var orderProduct = new OrderProduct
                    {
                        OrderId = order.Id,
                        ProductCode = product.ProductCode,
                        Quantity = _random.Next(1, 6) // Random quantity between 1 and 5
                    };
                    
                    orderProducts.Add(orderProduct);
                }
            }
            
            await _context.OrderProducts.AddRangeAsync(orderProducts);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Successfully generated {Count} order-product relationships", orderProducts.Count);
        }
    }
}
