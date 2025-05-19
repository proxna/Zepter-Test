using System.Text.Json;
using ZepterTest.Common.DTO;

namespace ZepterTest.MvcApplication.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderInfoDTO>> GetOrderInfoAsync()
        {
            List<OrderInfoDTO> orderInfo = null;
            var response = await _httpClient.GetAsync("/api/orderinfo");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                orderInfo = JsonSerializer.Deserialize<List<OrderInfoDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return orderInfo;
        }
    }
}