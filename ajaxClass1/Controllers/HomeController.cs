﻿using ajaxClass1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ajaxClass1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Json()
        {
            return View();
        }

        public IActionResult First()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Address()
        {
            return View();
        }

        public IActionResult Avatar()
        {
            return View();
        }

        public IActionResult Spots()
        {
            return View();
        }

        public IActionResult AutoComplete()
        {
            return View();
        }

        public IActionResult Cors()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
