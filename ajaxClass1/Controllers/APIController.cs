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
        private IWebHostEnvironment _environment;
        public APIController(MyDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
        public IActionResult Roads(string city, string district)
        {
            var roads = _context.Addresses
                .Where(c => c.City == city && c.SiteId == district)
                .Select(d => d.Road).Distinct();
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

        [HttpPost]                   //Member模型裡的屬性型別非IFormFile，在此另設一參數來處理
        public IActionResult Register(Member _user, IFormFile avatar)
        {
            //設定使用者姓名，預設值為 guest
            if (string.IsNullOrEmpty(_user.Name))
            {
                _user.Name = "guest";
            }

            //設定使用者頭像的檔案名，若無檔案則指定為 empty.png

            string fileName = "empty.png";
            if (avatar != null)
            {
                fileName = avatar.FileName;

                string[] type = { "image/jpeg", "image/jpeg", "image/png", "image/gif" };
                if(!type.Contains(avatar.ContentType))
                {
                    return Content("檔案格式非圖檔");
                }
            }

            
            //todo
            //1. 只允許上傳圖檔
            //2. 圖檔最大2M
            //3. 檔案名稱重複處理

            string filePath = Path.Combine(_environment.WebRootPath,"uploads", fileName);
            using (var filestream = new FileStream(filePath, FileMode.Create))
            {
                avatar?.CopyTo(filestream);
            }

            //將圖片新增到資料庫

            //存回 Member 屬性 FileName
            _user.FileName = fileName;

            //轉為二進位資料後，存回 Member 屬性 FileData
            byte[]? bytes = null;
            using (var memoryStream = new MemoryStream())
            {
                avatar.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            _user.FileData = bytes;

            _context.Members.Add(_user);
            _context.SaveChanges();

            return Content($"歡迎，{_user.Name}。您今年 {_user.Age} 歲，電子郵件是 {_user.Email}。", "text/plain", Encoding.UTF8);
        }

        public IActionResult CheckAccount(UserDTO _user)
        {
            var result = _context.Members.Any(m => m.Name == _user.Name);
            if (result == true)
            {
                return Content("帳號已存在", "text/plain", Encoding.UTF8);

            }
            return Content("帳號可使用", "text/plain", Encoding.UTF8);
        }


        [HttpPost]
        public IActionResult Spots([FromBody]SearchDTO _search)
        {
            //根據景點分類讀取資料
            var spots = _search.CategoryId == 0 ? _context.SpotImagesSpots : _context.SpotImagesSpots.Where(s=>s.CategoryId == _search.CategoryId);

            //根據關鍵字搜尋
            if(!string.IsNullOrEmpty(_search.Keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(_search.Keyword) ||
                                         s.SpotDescription.Contains(_search.Keyword));
            }

            //排序
            switch(_search.SortBy)
            {
                case "spotTitle":
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.SpotTitle) :
                                                        spots.OrderByDescending(s => s.SpotTitle);
                    break;
                case "categoryId":
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.CategoryId) :
                                                        spots.OrderByDescending(s => s.CategoryId);
                    break;
                default:
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.SpotId) :
                                                        spots.OrderByDescending(s => s.SpotId);
                    break;
            }

            //分頁
            int totalCount = spots.Count();
            int pageSize = _search.PageSize ?? 9;
            int totalPage = (int)Math.Ceiling((decimal)(totalCount / pageSize));
            int page = _search.Page ?? 1;
            spots = spots.Skip((int)(page - 1) * pageSize).Take(pageSize);


            //回傳資料
            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
            spotsPaging.TotalPages = totalPage;
            spotsPaging.SpotsResult = spots.ToList();

            return Json(spotsPaging);
        }

        public IActionResult SpotTitle(string title)
        {
            var titles = _context.Spots.Where(s => s.SpotTitle.Contains(title)).Select(s => s.SpotTitle).Take(8);
            return Json(titles);
        }

    }
}
