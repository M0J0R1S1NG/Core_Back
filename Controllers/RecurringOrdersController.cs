using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Helpers;
using Core;
using Core.Models.AccountViewModels;





namespace Core.Controllers
{
   [Authorize(Roles="Admin")]
    public class RecurringOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public RecurringOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory)
        {
            _context = context;   
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>(); 
        }
         public IActionResult index()
        {
            // Clear the existing external cookie to ensure a clean login process
            return View();
        }
         // POST: 
        
        [HttpPost]
        public IActionResult index( int empty = 0)
        {
            // Clear the existing external cookie to ensure a clean login process
            return View();
        }
    

    }
}