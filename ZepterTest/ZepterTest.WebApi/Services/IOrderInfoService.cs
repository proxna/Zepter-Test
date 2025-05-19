using ZepterTest.Common.DTO;

namespace ZepterTest.WebApi.Services
{
    public interface IOrderInfoService
    {
        public Task<List<OrderInfoDTO>> GetOrderInfoAsync();
    }
}
