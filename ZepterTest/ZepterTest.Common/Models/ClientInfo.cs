namespace ZepterTest.Common.Models
{
    public class ClientInfo
    {
        public long Id { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
