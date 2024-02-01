using Microsoft.AspNetCore.Mvc;

namespace ajaxClass1.Controllers
{
    public class HomeworkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Spot()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
