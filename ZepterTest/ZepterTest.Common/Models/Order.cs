using ZepterTest.Common.Enums;

namespace ZepterTest.Common.Models
{
    public class Order
    {
        public long Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ClientInfo ClientInfo { get; set; }
        public long ClientInfoId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public Shop Shop { get; set; }
        public int ShopId { get; set; }
    }
}
