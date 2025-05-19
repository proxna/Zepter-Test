using ZepterTest.Common.DTO;

namespace ZepterTest.MvcApplication.Models
{
    public class WebServiceViewModel
    {
        public List<OrderInfoDTO> Orders { get; set; } = new List<OrderInfoDTO>();
    }
}
