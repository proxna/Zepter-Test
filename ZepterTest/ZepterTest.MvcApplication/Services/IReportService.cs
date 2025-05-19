using ZepterTest.Common.DTO;

namespace ZepterTest.MvcApplication.Services
{
    public interface IReportService
    {
        List<OrderReportDTO> GetOrderReports();
    }
}
