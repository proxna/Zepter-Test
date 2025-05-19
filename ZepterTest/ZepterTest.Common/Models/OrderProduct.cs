namespace ZepterTest.Common.Models
{
    public class OrderProduct
    {
        public Guid ProductCode { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; }
    }
}
