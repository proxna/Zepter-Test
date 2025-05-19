using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ZepterTest.Common.DTO;
using ZepterTest.MvcApplication.Models;
using ZepterTest.MvcApplication.Services;

namespace ZepterTest.MvcApplication.Controllers
{
    public class WebServiceController : Controller
    {
        private readonly IApiService apiService;
        public WebServiceController(IApiService apiService)
        {
            this.apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var orderInfo = await apiService.GetOrderInfoAsync();
            return View(new WebServiceViewModel { Orders = orderInfo });
        }
    }
}
