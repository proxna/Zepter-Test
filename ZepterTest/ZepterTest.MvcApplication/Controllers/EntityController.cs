using Microsoft.AspNetCore.Mvc;

namespace ZepterTest.MvcApplication.Controllers
{
    public class EntityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
