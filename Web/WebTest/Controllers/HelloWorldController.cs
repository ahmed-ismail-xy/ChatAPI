﻿using Microsoft.AspNetCore.Mvc;

namespace WebTest.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
