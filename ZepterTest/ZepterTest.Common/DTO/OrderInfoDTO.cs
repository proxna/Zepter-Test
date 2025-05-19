using ZepterTest.Common.Enums;

namespace ZepterTest.Common.DTO
{
    public class OrderInfoDTO
    {
        public long Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public decimal NetTotal { get; set; }
    }
}
