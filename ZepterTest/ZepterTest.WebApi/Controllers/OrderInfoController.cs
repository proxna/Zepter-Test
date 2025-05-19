using Microsoft.AspNetCore.Mvc;
using ZepterTest.WebApi.Services;

namespace ZepterTest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderInfoController : ControllerBase
    {
        private readonly IOrderInfoService _orderInfoService;
        public OrderInfoController(IOrderInfoService orderInfoService)
        {
            _orderInfoService = orderInfoService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderInfo()
        {
            var result = await _orderInfoService.GetOrderInfoAsync();
            return Ok(result);
        }
    }
}
