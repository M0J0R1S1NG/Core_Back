using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration.Json;

using System.Security.Claims;

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
    public class DebitWayController : Controller
    {
         private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public DebitWayController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
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

        // GET: DebitWay
        public async Task<IActionResult> Index()
        {
            return View(await _context.DebitWay.ToListAsync());
        }

        // GET: DebitWay/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWay = await _context.DebitWay
                .SingleOrDefaultAsync(m => m.ID == id);
            if (debitWay == null)
            {
                return NotFound();
            }

            return View(debitWay);
        }

        // GET: DebitWay/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DebitWay/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Create([Bind("ID,identifier,vericode,website_unique_id,return_url,transaction_id,merchant_transaction_id,transaction_status,transaction_result,transaction_date,transaction_type,item_name,amount,quantity,item_code,language,email,phone,custom,shipment,first_name,last_name,address,city,state_or_province,zip_or_postal_code,country,shipping_address,shipping_city,shipping_state_or_province,shipping_zip_or_postal_code,shipping_country,customer_errors_meaning,errors,issuer_name,issuer_confirmation,status,time_stamp")] DebitWay debitWay)
        {
            if (ModelState.IsValid)
            {
                if (debitWay.transaction_result!="success"){
                    ViewData["message"]="Failed Transaction";

                    return RedirectToAction("forsale","Inventorys");

                }


                debitWay.transaction_date_datetime=DateTime.Now;
                _context.Add(debitWay);
                await _context.SaveChangesAsync();
                
                 string[] customString = debitWay.custom.Split(',');
                 string userId=customString[0].Trim();
                 string SpecialInstructions=customString[1];
                

      
                 Order thisOrder = _context.Orders.Where(o=> o.GUID == Guid.Parse(debitWay.merchant_transaction_id)).First();
                var user = await _userManager.FindByIdAsync(userId);
                //var user = await _userManager.FindByNameAsync(User.Identity.Name);
                // thisOrder.AppUser= Guid.Parse(user.Id);
                // thisOrder.OrderDate = DateTime.Now;
                // if (thisOrder.GeocodedAddress==null || thisOrder.GeocodedAddress==""){thisOrder.GeocodedAddress= user.DeliveryAddress;}
                // if (thisOrder.SpecialInstructions.Contains("Must add")) {thisOrder.SpecialInstructions= "";}
                // //_context.Add(order);

                await _context.SaveChangesAsync();
                string message = "<p>Thank You, we got your order.</p>We are delivering the following: " +  thisOrder.Details +  " <br>To: " ;
                message += thisOrder.GeocodedAddress + "<br>";
                message+= "Special Instructions:" + thisOrder.SpecialInstructions + "<br>";
                //message += "The total for this order is " +  order.Total.ToString() + "<br>";
                message += "<p>Your order number is " + thisOrder.ID + "-" + thisOrder.AppUser + "</p>";
                
                string smsmessage="Thanks we got your order. We are delivering the following: " +  thisOrder.Details + (char)10 + (char)13 + "To: "  ;
                smsmessage += thisOrder.GeocodedAddress + (char)10 + (char)13;
                smsmessage+= "Special Instructions:" + thisOrder.SpecialInstructions + (char)10 + (char)13;
                //smsmessage += "The total for this order is " +  order.Total.ToString() + (char)10 + (char)13;
                smsmessage += "Your order number is " + thisOrder.ID + "-" + thisOrder.AppUser ;


                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), smsmessage);
                //await _smsSender.SendSmsAsync("6475284350", _userManager.GetPhoneNumberAsync(user).Result + " " + thisOrder.GeocodedAddress + " " +  thisOrder.Details);
                await _smsSender.SendSmsAsync("4168028129", _userManager.GetPhoneNumberAsync(user).Result + " " + thisOrder.GeocodedAddress + " " +  thisOrder.Details);
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "New Order", message);
                //await _emailSender.SendEmailAsync("andrewmoore46@gmail.com",  _userManager.GetPhoneNumberAsync(user).Result + " " + "New Order", message);
                await _emailSender.SendEmailAsync("moorea@uberduber.com",  _userManager.GetPhoneNumberAsync(user).Result + " " + "New Order", message);
                //var item = new JsonResult( _context.Orders.SingleOrDefaultAsync());














                
                return RedirectToAction("Index","orders");
            }
            return View(debitWay);
        }

        // GET: DebitWay/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWay = await _context.DebitWay.SingleOrDefaultAsync(m => m.ID == id);
            if (debitWay == null)
            {
                return NotFound();
            }
            return View(debitWay);
        }

        // POST: DebitWay/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,identifier,vericode,website_unique_id,return_url,transaction_id,merchant_transaction_id,transaction_status,transaction_result,transaction_date,transaction_date_datetime,transaction_type,item_name,amount,quantity,item_code,language,email,phone,custom,shipment,first_name,last_name,address,city,state_or_province,zip_or_postal_code,country,shipping_address,shipping_city,shipping_state_or_province,shipping_zip_or_postal_code,shipping_country,customer_errors_meaning,errors,issuer_name,issuer_confirmation,status,time_stamp")] DebitWay debitWay)
        {
            if (id != debitWay.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(debitWay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DebitWayExists(debitWay.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(debitWay);
        }

        // GET: DebitWay/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWay = await _context.DebitWay
                .SingleOrDefaultAsync(m => m.ID == id);
            if (debitWay == null)
            {
                return NotFound();
            }

            return View(debitWay);
        }

        // POST: DebitWay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var debitWay = await _context.DebitWay.SingleOrDefaultAsync(m => m.ID == id);
            _context.DebitWay.Remove(debitWay);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DebitWayExists(int id)
        {
            return _context.DebitWay.Any(e => e.ID == id);
        }
    }
}
