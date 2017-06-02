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
    [Authorize]
    public class ShoppingController : Controller
 {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public ShoppingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
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

        // GET: Orders
        public async Task <IActionResult> Index()
        {   var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null){
                if (user.DeliveryAddress == null){
                    return RedirectToAction("UpdateUser","Manage");
                }
                ViewData["DeliveryAddress"]=user.DeliveryAddress;
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, $"You must have a confirmed email to log in. The confirmation email has been re-sent to {User.Identity.Name}");
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                        await _emailSender.SendEmailAsync(User.Identity.Name, "Confirm your account", $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                        return View();
                    }
                if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
                    {
                        return RedirectToAction("AddPhoneNumber","Manage");
                    }  
                // if (!user.Mnp)
                //     {
                //         ViewData["Title"]="Membership Purchase";
                //         return RedirectToAction("create","MnpForm");
                //     } 
                if (  user.status==0)
                    {
                        ViewData["Title"]="Membership Purchase";
                        return RedirectToAction("PurchaseMembership","Shopping");
                    } 
                return View();
            }else{
                return RedirectToAction("Register","Account");
            }
        }
         public async Task <IActionResult> PurchaseMembership(){
            
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.status!=0){
                return RedirectToAction("Shopping");
            }
            return View();

         }
        public async Task <IActionResult> PurchaseConfirmed(){
            
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.status = 1;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return View();

         }
        public IActionResult PurchaseCancelled(){
           
            ViewData["Message"]="";
            return View();

         }
        public IActionResult Default()
        {
            return View(_context.Orders.ToList());
        }
         public IActionResult orderPayed()
        {
            return View();
        }
    }
}