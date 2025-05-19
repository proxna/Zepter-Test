using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ZepterTest.Common.DTO;
using ZepterTest.MvcApplication.Models;

namespace ZepterTest.MvcApplication.Controllers
{
    public class WebServiceController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<OrderInfoDTO> orderInfo = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7137/api/orderinfo");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    orderInfo = JsonSerializer.Deserialize<List<OrderInfoDTO>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }

            return View(new WebServiceViewModel { Orders = orderInfo });
        }
    }
}
