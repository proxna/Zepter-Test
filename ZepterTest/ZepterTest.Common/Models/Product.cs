namespace ZepterTest.Common.Models
{
    public class Product
    {
        public Guid ProductCode { get; set; }

        public decimal Price { get; set; }

        public decimal Vat { get; set; } = 0;

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
