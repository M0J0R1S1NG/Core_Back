﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;




namespace Core.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger _logger;
        private readonly IOptions<SiteSettings> config;
        public HomeController(IOptions<SiteSettings> config, ILoggerFactory loggerFactory)
        {
            this.config = config;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        public IActionResult Index()
        {
            try {
                if ( !HttpContext.Request.Host.Host.Contains("uberduber")){
                    return RedirectPermanent("https://www.uberduber.com");
                }
            }catch(System.NullReferenceException){
                 _logger.LogInformation(1, "Null Exception on Home/Index");
            }
            return View();
        }
        [Authorize(Roles="Admin")]
        public IActionResult Admin()
        {
           
            return View();
        }
        [Authorize(Roles="Admin,Investor,Partner")]
        public IActionResult Investors()
        {
           return View();
        }
        public IActionResult Default()
        {
            return View();
        }
         public IActionResult Disclaimer()
        {
            return View();
        }
        public IActionResult About()
        { 
            ViewData["DomainName"]=HttpContext.Request.Host;
            ViewData["Message"] ="";
            ViewData["Title"] ="About us...";
          
           
            return View();
        }
        public IActionResult Shipping()
        { 
            ViewData["DomainName"]=HttpContext.Request.Host;
            ViewData["Message"] ="24 hour local dispatched delivery and mail order services";
            ViewData["Title"] ="Shipping and Deliveries";
          
           
            return View();
        }
         public IActionResult Downloads()
        { 
            ViewData["DomainName"]=HttpContext.Request.Host;
            ViewData["Message"] ="";
            ViewData["Title"] ="Downloads";
          
           
            return View();
        }
         public IActionResult Info()
        { 
            ViewData["DomainName"]=HttpContext.Request.Host;
            ViewData["Message"] ="";
            ViewData["Title"] ="Info";
          
           
            return View();
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "";
            //ViewData["WebDomain"] = Configuration["SiteSettings:WebDomain"];
            ViewData["ContactEmail"] =config.Value.ContactEmail;
            ViewData["SupportEmail"] =config.Value.SupportEmail;
            ViewData["SalesEmail"] = config.Value.SalesEmail;
            ViewData["ContactPhoneNumber"] = config.Value.ContactPhoneNumber;
            ViewData["CompanyName"] = config.Value.CompanyName;
            return View();
        }
        public IActionResult Returns()
        {
            ViewData["Message"] = "";
            ViewData["ContactPhoneNumber"] = "1-647-799-2699";
            ViewData["EmailSupport"] = "support@dubes.com";
            
            return View();
        }
        public IActionResult Medical()
        {
            ViewData["Message"] = "";
            ViewData["Title"] = "Current Medical Marijuana Status";
          
            
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
