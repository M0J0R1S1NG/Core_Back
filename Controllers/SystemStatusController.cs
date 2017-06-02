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
using Core.Models.AccountViewModels;
using Core.Services;
using Core.Data;
namespace Core.Controllers
{
    [Authorize]
    public class SystemStatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;

        public SystemStatusController(ApplicationDbContext context,
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
    

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> index()
        {
            // Clear the existing external cookie to ensure a clean login process
            
            
            var UserId = _userManager.GetUserId(User);
        if (UserId != null){
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["Email"]= User.Identity.Name;
            ViewData["Mnp"]=user.Mnp;
            ViewData["IsLoggedIn"] = User.Identity.IsAuthenticated;
            ViewData["User"] = User;
            ViewData["IsSignedIn"] =  _signInManager.IsSignedIn(User);
            ViewData["ConfirmedPhone"] =  _userManager.IsPhoneNumberConfirmedAsync(user).Result;
            ViewData["ConfirmedEmail"] =  _userManager.IsEmailConfirmedAsync(user).Result;
            ViewData["MemberPaid"] =  user.status;
            ViewData["profileCreated"] = (user.DeliveryAddress!=null);
            ViewData["FailedAccessCount"] =  user.AccessFailedCount;
            ViewData["LockoutEndDate"] =  user.LockoutEnd;
            ViewData["AccessFailedCount"] =  user.AccessFailedCount;
            ViewData["PasswordConfirmed"] =  user.PasswordHash;
        }else{
            ViewData["LockoutEndDate"] =DateTime.Now;
        }
            
            ViewData["SystemStatus"]="We are open for business current deliveries are ~20 minutes";
            
            return View();
        }
    }
}