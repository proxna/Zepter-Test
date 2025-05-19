namespace ZepterTest.DataWriter.Config
{
    /// <summary>
    /// Configuration for data generators
    /// </summary>
    public class DataGeneratorConfig
    {
        /// <summary>
        /// Number of shops to generate
        /// </summary>
        public int ShopCount { get; set; } = 10;
        
        /// <summary>
        /// Number of clients to generate
        /// </summary>
        public int ClientCount { get; set; } = 50;
        
        /// <summary>
        /// Number of products to generate
        /// </summary>
        public int ProductCount { get; set; } = 100;
        
        /// <summary>
        /// Number of orders to generate
        /// </summary>
        public int OrderCount { get; set; } = 200;
        
        /// <summary>
        /// Maximum number of products per order
        /// </summary>
        public int MaxProductsPerOrder { get; set; } = 5;
    }
}
