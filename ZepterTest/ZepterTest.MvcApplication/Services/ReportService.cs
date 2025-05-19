using ZepterTest.Common;
using ZepterTest.Common.DTO;
using ZepterTest.Common.Enums;

namespace ZepterTest.MvcApplication.Services
{
    public class ReportService : IReportService
    {
        private readonly ZepterTestContext _context;

        public ReportService(ZepterTestContext context)
        {
            _context = context;
        }

        public List<OrderReportDTO> GetOrderReports()
        {
            List<OrderReportDTO> orderReports = new List<OrderReportDTO>();

            foreach(PaymentMethod paymentMethod in Enum.GetValues(typeof(PaymentMethod)))
            {
                var orders = _context.Orders
                    .Where(o => o.PaymentMethod == paymentMethod);
                orders = orders.Where(o => o.Products.Select(p => p.Price * (1 + p.Vat)).Sum() > 100);
                var orderReport = new OrderReportDTO
                {
                    PaymentMethod = paymentMethod,
                    OrdersCount = orders.Count(),
                    TotalGrossValue = orders.Sum(o => o.Products.Select(p => p.Price * (1 + p.Vat)).Sum())
                };
                orderReports.Add(orderReport);
            }

            return orderReports;
        }
    }
}
