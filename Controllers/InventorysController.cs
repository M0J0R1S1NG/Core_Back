using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Models.AccountViewModels;
using Core.Services;



namespace Core.Controllers
{
   [Authorize(Roles="Admin")]
    public class InventorysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;

        public InventorysController(ApplicationDbContext context,
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

        // GET: Inventorys
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventorys.ToListAsync());
        }

        // GET: Inventorys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventorys
                .SingleOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventorys/Create
        public async Task<IActionResult> Create()
        {
          
            return View();
        }

        // POST: Inventorys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Label,Description,Status,OnHand,BestBefore,OrderDate,Quantity,Price,Cost,Supplier,Notes,Photo,THCContent,CBDContent,Likes,ImageFilePath,PricePerGram,PricePerQuarter,PricePerhalf,PricePerOz,CostPerGram,Discount,UPC,Qualities,catagory")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inventory);
        }

        // GET: Inventorys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventorys.SingleOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }  
            List<string> domains = new List<string>();
            domains.Add("DomainA");
            domains.Add("DomainB");
            string[] myvar2 = {"Concentrates","Flowers","Edibales"};

            domains.Select(m => new SelectListItem { Text = m, Value = m });

            ViewBag.Domains = domains;
            ViewBag.query = _context.Inventorys.Where(c=> c.Status==1);
            var Products = _context.Inventorys.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Label });
            var mylist =  domains.Select(x=> new {Id=x,ValueTask=x} );
            var mylist2 = myvar2.Select(m => new SelectListItem { Text = m, Value = m });
           // ViewBag.query = new SelectList(Products, "Id", "Value");
           
            ViewBag.MyList2=mylist2;
                //Use the following view syntax  when the return is like MyList2 or query 
                //<select asp-for="Label" asp-items=ViewBag.query></select   >  
                
                //use the following view syntax with mylist
                ViewBag.MyList = mylist;
                //@Html.DropDownList("domains", ((List<string>)ViewBag.domains).Select(m => new SelectListItem { Text = m, Value = m }))



            // var statuses = from Inventory s in _context.Inventorys.AsEnumerable()  select new { ID = s.ID, Name = s.Label,Value = s.ID };
            // ViewData["InventoryStatus"] = new SelectList(statuses);
            return View(inventory);
        }

        // POST: Inventorys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Label,Description,Status,OnHand,BestBefore,OrderDate,Quantity,Price,Cost,Supplier,Notes,Photo,THCContent,CBDContent,Likes,ImageFilePath,PricePerGram,PricePerQuarter,PricePerhalf,PricePerOz,CostPerGram,Discount,UPC,Qualities,catagory")] Inventory inventory)
        {
            if (id != inventory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.ID))
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
            return View(inventory);
        }

        // GET: Inventorys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventorys
                .SingleOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventorys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventorys.SingleOrDefaultAsync(m => m.ID == id);
            _context.Inventorys.Remove(inventory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventorys.Any(e => e.ID == id);
        }
    }
}
