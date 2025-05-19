using ZepterTest.Common.Enums;

namespace ZepterTest.Common.DTO
{
    public class OrderReportDTO
    {
        public PaymentMethod PaymentMethod { get; set; }
        public int OrdersCount { get; set; }
        public decimal TotalGrossValue { get; set; }
    }
}
