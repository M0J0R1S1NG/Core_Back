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
   

    [Authorize]
    public class MnpFormController : Controller
    {   private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;

        public MnpFormController(
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
        public IActionResult index()
        {
            IEnumerable<MnpForm> mything = _context.MnpForms.AsEnumerable();
            
            // Clear the existing external cookie to ensure a clean login process
            return View(mything.Where(c=>c.UserId==Guid.Parse(_userManager.GetUserId(User))));
        }
        
        [HttpGet]
        public IActionResult create()
        {   
            var UserId=  _userManager.GetUserId(User);
            ApplicationUser myUser = _context.Users.Where(ci => ci.Id== UserId).First();
            MnpForm mything = _context.MnpForms.Where(c=>c.UserId==Guid.Parse(_userManager.GetUserId(User))).SingleOrDefault();
            // Clear the existing external cookie to ensure a clean login process
            ViewData["UserHasForm"] = myUser.Mnp;
            return View(mything);
        }
        [HttpPost]
        public async Task<IActionResult> create([Bind("UserId,MedConditionsExisting,MedConditions,MedConditionsTreatment,MedCannabisUser,YearsUsing,RelievesCondition,HasAlergies,Alergies,Signature,SignatureFile,Date,PhotoId,Image,ImageName,LegalFirstName,LegalMiddleName,LegalLastName,Doctor,DoctorCity,Dispencery,WantRx")] MnpForm model)
        {
            
           
            if (ModelState.IsValid)
            {
                    //Submit new record
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    //Update user to show the have filled out the MnpForm
                    var UserId=  _userManager.GetUserId(User);
                    ApplicationUser myUser = _context.Users.Where(ci => ci.Id== UserId).First();   
                    myUser.Mnp=true;
                    _context.Update(myUser);
                    await _context.SaveChangesAsync();
                    ViewData["UserHasForm"] = myUser.Mnp;
                    return RedirectToAction("Create");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult edit()
        {   
            MnpForm mything = _context.MnpForms.Where(c=>c.UserId==Guid.Parse(_userManager.GetUserId(User))).SingleOrDefault();
            return View(mything);
        }

        [HttpPost]
        public async Task<IActionResult> edit([Bind("Id,UserId,MedConditionsExisting,MedConditions,MedConditionsTreatment,MedCannabisUser,YearsUsing,RelievesCondition,HasAlergies,Alergies,Signature,SignatureFile,Date,PhotoId,Image,ImageName,LegalFirstName,LegalMiddleName,LegalLastName,Doctor,DoctorCity,Dispencery,WantRx")] MnpForm model)
        {
            if (ModelState.IsValid)
            {
                 _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return NotFound();
        }
         private bool FormExists(int id)
        {
            return _context.MnpForms.Any(e => e.Id == id);
        }
    }
}
 

