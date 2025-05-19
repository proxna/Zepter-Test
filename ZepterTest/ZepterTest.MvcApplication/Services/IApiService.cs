using ZepterTest.Common.DTO;

namespace ZepterTest.MvcApplication.Services
{
    public interface IApiService
    {
        Task<List<OrderInfoDTO>> GetOrderInfoAsync();
    }
}
