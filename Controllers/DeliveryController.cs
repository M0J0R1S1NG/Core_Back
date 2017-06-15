using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Models;
using Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Models.ManageViewModels;
using Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Core.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {   
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;



        public DeliveryController(ApplicationDbContext context,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory)
        {   _context = context;   
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
        }
        [AllowAnonymous]
        public async Task <IActionResult> Index()
        {   
            ViewBag.emailAddress=ViewBag.emailAddress;
            ViewBag.searchBox=ViewBag.searchBox;
           ViewBag.IsAdmin=User.IsInRole("Admin");           
            List<DeliveryArea> deliveryareas = _context.DeliveryAreas.Where(c=> c.Status>=0).ToList();
            return View(deliveryareas);
        }
        [AllowAnonymous]
        public IActionResult Default()
        {
            return View();
        }
        
         [HttpGet]
         [AllowAnonymous]
         public ActionResult ModalAction(int Id=0,string title="No Title",string message="No Message")
        {

            ViewBag.Id = Id;
            ViewBag.Message=message;
            ViewBag.Title=title;
            return PartialView("ModalContent", new BootstrapModel { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task <IActionResult> AddEmail(string searchBox, string emailAddress)
        {
           TempData["emailAddress"] =emailAddress;

           
            Address newadd = new Core.Models.Address();
            newadd.Name = emailAddress;
            newadd.FullAddress = searchBox;
            newadd.CreatedUser= Guid.NewGuid();
            newadd.Status = 3;
            newadd.Geocoded = true;
            await _context.Addresses.AddAsync(newadd);
            await _context.SaveChangesAsync();

            List<DeliveryArea> deliveryareas = _context.DeliveryAreas.Where(c=> c.Status>=0).ToList();
            return RedirectToAction("index");

        }
    }
}

