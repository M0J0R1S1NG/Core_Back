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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
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
        public async Task<IActionResult> Index()
        {var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userGuid = Guid.Parse(user.Id);
            var order =  (_context.Orders).Where(m => m.AppUser == userGuid );
            return View(order);
        }

        // GET: Orders/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userGuid = Guid.Parse(user.Id);
            var order = await _context.Orders
                .SingleOrDefaultAsync(m => m.ID == id && m.AppUser == userGuid );
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID,CustomerId,OrderDate,DeliveryDate,GeocodedAddress,Total,Weight,PaymentType,Details,SpecialInstructions,Status,DriverId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [HttpGet]
        public IActionResult CreateApi()
        {
            
            return  View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateApi([Bind("Total,GeocodedAddress,Weight,PaymentType,Details,SpecialInstructions,Status,DriverId,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
             {   
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                order.AppUser= Guid.Parse(user.Id);
                order.GeocodedAddress= user.DeliveryAddress;
                _context.Add(order);

                await _context.SaveChangesAsync();
                string message = "Thanks You, we got your order. We are delivering the following: "  + (char)10 +(char)13 + order.Details + (char)10 +(char)13 +  "To: " ;
                message += order.GeocodedAddress + (char)10 + (Char)13  ;
                message += "You can edit the delivery address up until we dispatch your order."  +  (char)10 + (Char)13;
                message += "The total for this order is " +  order.Total.ToString() + " + tax"+  (char)10 + (Char)13;
                message += "Your order number is " + order.ID + "-" + order.AppUser;
                
                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
                await _smsSender.SendSmsAsync("6475284350", _userManager.GetPhoneNumberAsync(user) + " " + order.GeocodedAddress + " " +  order.Details);
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "New Order", message);
                await _emailSender.SendEmailAsync("a2bman@hotmail.com",  _userManager.GetPhoneNumberAsync(user) + " " + "New Order", message);
                //var item = new JsonResult( _context.Orders.SingleOrDefaultAsync());
                return StatusCode(200);
            }
            
            return StatusCode(404);
        }
        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userGuid = Guid.Parse(user.Id);
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id && m.AppUser==userGuid);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        
        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("ID,CustomerId,OrderDate,DeliveryDate,GeocodedAddress,Total,Weight,PaymentType,Details,SpecialInstructions,Status,DriverId")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}
