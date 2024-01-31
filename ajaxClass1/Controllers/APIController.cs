using ajaxClass1.Models;
using ajaxClass1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text;

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
            //int x = 10;
            //int y = 0;
            //int z = x / y;
            Thread.Sleep(50000);
            return Content("Hi 我來自 APIController！", "text/plain", Encoding.UTF8);
        }

        //讀取城市資料
        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(c => c.City).Distinct();
            return Json(cities);
        }

        //根據城市讀取鄉鎮區
        public IActionResult Districts(string city)
        {
            var districts = _context.Addresses.Where(c => c.City == city).Select(d=> d.SiteId).Distinct();
            return Json(districts);
        }

        //根據鄉鎮區讀取路名
        public IActionResult Roads(string district)
        {
            var roads = _context.Addresses.Where(c => c.SiteId == district).Select(d => d.Road).Distinct();
            return Json(roads);
        }

        public IActionResult Avatar(int id = 1)
        {
            Member? member = _context.Members.Find(id);
            if(member != null)
            {
                byte[] avatar = member.FileData;
                if(avatar != null)
                {
                    return File(avatar, "image/jpeg");
                }
            }
            return NotFound();
        }

        //public IActionResult Register(string name, int age = 20)
        //{
        //    if(string.IsNullOrEmpty(name))
        //    {
        //        name = "來賓";
        //    }
        //    return Content($"歡迎，{age}歲的{name}！", "text/plain", Encoding.UTF8);
        //}

        public IActionResult Register(UserDTO _user)
        {
            if (string.IsNullOrEmpty(_user.Name))
            {
                _user.Name = "guest";
            }
            return Content($"歡迎，{_user.Name}。您今年 {_user.Age} 歲，電子郵件是 {_user.Email}。", "text/plain", Encoding.UTF8);
        }

        public IActionResult CheckAccount(UserDTO user)
        {
            var result = _context.Members.Any(m => m.Name == user.Name);
            if (result == true)
            {
                return Content("帳號已存在", "text/plain", Encoding.UTF8);

            }
            return Content("帳號可使用", "text/plain", Encoding.UTF8);
        }
    }
}
