using Microsoft.AspNetCore.Mvc;
using ZepterTest.MvcApplication.Services;

namespace ZepterTest.MvcApplication.Controllers
{
    public class EntityController : Controller
    {
        private readonly IReportService _reportService;

        public EntityController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            var orderReports = _reportService.GetOrderReports();
            return View(orderReports);
        }
    }
}
