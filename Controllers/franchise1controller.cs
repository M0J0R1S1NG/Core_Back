using System;

using System.Collections.Generic;

using System.Linq;

using System.Security.Claims;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;

using Core.Models;

using Core.Services;

using Core.Data;

using Core.Models.ManageViewModels;

using Core.Models.MnpFormViewModels;

using Microsoft.EntityFrameworkCore;



namespace Core.Controllers

{

    

    public class Franchise1Controller : Controller

    {   private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IEmailSender _emailSender;

        private readonly ISmsSender _smsSender;

        private readonly ILogger _logger;

        private readonly string _externalCookieScheme;



        public Franchise1Controller(

            ApplicationDbContext context,

            UserManager<ApplicationUser> userManager,

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

            _logger = loggerFactory.CreateLogger<AccountController>();

        }



        //

        // GET: 

        [HttpGet]

        public IActionResult ThankYou()

        {

            // Clear the existing external cookie to ensure a clean login process

            return View();

        }

        [HttpGet]

        

        public IActionResult index()

        {

            // Clear the existing external cookie to ensure a clean login process

            return View();

        }

        [Authorize]

        [HttpGet]

        public async Task<IActionResult> Apply()

        {   var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!await _userManager.IsPhoneNumberConfirmedAsync(user))

                    {

                        return View();

                    } 

            var UserId=  _userManager.GetUserId(User);

            Franchise mything = _context.Franchise.Where(c=>c.AppUser==Guid.Parse(UserId)).SingleOrDefault();

            

            return View(mything);

            

        }

        [Authorize]

        [HttpPost]

        public async Task<IActionResult> Apply([Bind("AppUser,City,ContractDate,HasVehicle,VehcileType,VehcileYear,Consent,StartDate")] Franchise model)

        {   var user = await _userManager.FindByNameAsync(User.Identity.Name);

           
            if (ModelState.IsValid)

            {   //addnewrecord

                model.AppUser = Guid.Parse(user.Id) ;

                _context.Add(model);

                await _context.SaveChangesAsync();

                return RedirectToAction("ThankYou");

            }

            return View(model);

        }

    }

}