using Microsoft.AspNetCore.Mvc;

namespace APIsEx.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
