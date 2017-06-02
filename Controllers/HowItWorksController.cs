using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Core.Controllers
{
    
    public class HowItWorksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Default()
        {
            return View();
        }
    }
}