namespace ZepterTest.Common.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
