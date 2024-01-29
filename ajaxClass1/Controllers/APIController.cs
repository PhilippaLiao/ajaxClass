using ajaxClass1.Models;
using Microsoft.AspNetCore.Mvc;

namespace ajaxClass1.Controllers
{
    public class APIController : Controller
    {
        private MyDBContext _context;
        public APIController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(c => c.City).Distinct();
            return Json(cities);
        }
    }
}
