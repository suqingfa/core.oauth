using Microsoft.AspNetCore.Mvc;

namespace MvcServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
