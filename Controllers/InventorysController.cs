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
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;



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
        
        [Authorize]
        [HttpGet]
        
        public async Task<IActionResult> ForSale()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userGuid = Guid.Parse(user.Id);

            DebitWay myDebitWay = new Models.DebitWay();
            myDebitWay.website_unique_id = "spJx8tqQJYjKRw24";
             myDebitWay.address = user.DeliveryAddress;
             myDebitWay.city = user.City;
             myDebitWay.state_or_province = user.Province;
             myDebitWay.country = user.Country;
             myDebitWay.zip_or_postal_code = user.PostalCode;
             myDebitWay.first_name = user.FirstName;
             myDebitWay.last_name=user.LastName;
             myDebitWay.language= user.Language;
             myDebitWay.merchant_transaction_id = Guid.NewGuid().ToString();
            myDebitWay.phone=user.PhoneNumber;
            myDebitWay.email=user.Email;
            myDebitWay.shipping_address=user.DeliveryAddress;
            myDebitWay.shipping_city=user.City;
            myDebitWay.shipping_state_or_province=user.Province;
            myDebitWay.shipping_country=user.Country;
            myDebitWay.shipping_zip_or_postal_code=user.PostalCode;
            myDebitWay.status="init";
            myDebitWay.custom=userGuid.ToString();
            myDebitWay.shipment="yes";
            myDebitWay.transaction_date=DateTime.Now.ToString();
            myDebitWay.transaction_id="";
            myDebitWay.time_stamp=DateTime.Now;
            myDebitWay.return_url="Https://" +Request.Host  + "/DebitWay/Create";
            
            ViewBag.DebitWay=myDebitWay;
           
    
            ViewBag.user = user;
            ViewBag.UserId = _userManager.GetUserId(User);
            
            
           ViewBag.thisUser=user;
             ViewData["StreetNumber"]=user.StreetNumber;
             ViewData["DeliveryAddress"]=user.DeliveryAddress;
             ViewData["StreetName"]=user.StreetName;
             ViewData["City"]=user.City;
             ViewData["Country"]=user.Country;
             ViewData["PostalCode"]=user.PostalCode;
             ViewData["Province"]=user.Province;

            var Products = _context.Inventorys.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Label });
            var  UserAreaId = _context.Users.Where(x=> x.Id==userGuid.ToString()).OrderBy(c => c.LastName).Select(x => new {x.DeliveryAreaId }).Single();
            ViewBag.query2 = new SelectList(Products, "Id", "Value");
            ViewBag.Inventory = _context.Inventorys.Where(c=> c.Status>0);
            ViewBag.drivers = _context.Drivers.Where(c=> c.Status>0);
            ViewBag.partners= _context.Partners.Where(c=> c.Status>0);
            ViewBag.deliveryareas = _context.DeliveryAreas.Where(c=> c.Status>=0);
            dynamic deliveryAreaName = from da in  _context.DeliveryAreas where da.ID==UserAreaId.DeliveryAreaId select  new DeliveryArea { Name= da.Name,ID=da.ID,OpenTime=da.OpenTime,ClosedTime=da.ClosedTime };
            
            
            ViewBag.deliveryAreaName =deliveryAreaName;
            ViewBag.inventorygroups = _context.InventoryGroups;
            
            
            var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreasForSelect = new SelectList(deliveryareaGuids, "Id", "Value");
            
            dynamic  InventoryByAreaVar = from d in _context.Inventorys
                                        join ig in _context.InventoryGroups on  d.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        //join pa in _context.Partners on da.ID equals pa.Id  
                                        //join us in _context.Users on pa.GUID equals us.Id
                                        
                                        //where da.ID == UserAreaId.DeliveryAreaId
                                        where d.Quantity > 0
                                        orderby da.Name
                                        select  new InventoryByArea
                                        {
                                           Label=d.Label, Name=da.Name,ID=da.ID,Quantity= d.Quantity,Price=d.Price,ImageFilePath=d.ImageFilePath, InventoryId=d.ID
                                        };


                                        ViewBag.InventoryByArea = InventoryByAreaVar;

            return View(myDebitWay);

            
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
           string[] myvar2 = {"Concentrates","Flowers","Edibales","Topicals","Hashish","Oils","Paraphanalia"};
            ViewBag.MyList2=  myvar2.Select(m => new SelectListItem { Text = m, Value = m });
           
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
            string[] myvar2 = {"Concentrates","Flowers","Edibales","Topicals","Hashish","Oils","Paraphanalia"};

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

         public async Task<IActionResult> byPartner(int? id)
        {
        Partner thisPartner=await _context.Partners.Where(z=> z.Id==id).SingleAsync();
        int PartnerManagedAreaId=thisPartner.DeliveryArea;
        
            if (id == null)
            {
                return NotFound();
            }
            IQueryable<Core.Models.Inventory> inventory =   from d in _context.Inventorys
                                        join ig in _context.InventoryGroups on  d.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        join pa in _context.Partners on da.PartnerId equals pa.Id
                                        join us in _context.Users on pa.GUID equals Guid.Parse(us.Id)  //grap the user stuff
                                        where pa.Id == thisPartner.Id
                                        
                                        orderby da.Name
                                        select  new Inventory
                                        {
                                           Label=d.Label,ID=d.ID,Quantity= d.Quantity,Price=d.Price,ImageFilePath=d.ImageFilePath,Status=d.Status,OnHand=d.OnHand,BestBefore=d.BestBefore,OrderDate=d.OrderDate,Cost=d.Cost,Supplier=d.Supplier,Notes=d.Notes,Photo=d.Photo,THCContent=d.THCContent,CBDContent=d.CBDContent,Likes=d.Likes,PricePerGram=d.PricePerGram,PricePerQuarter=d.PricePerQuarter,PricePerhalf=d.PricePerhalf,PricePerOz=d.PricePerOz,CostPerGram=d.CostPerGram,Discount=d.Discount,UPC=d.UPC,Qualities=d.Qualities,catagory=d.catagory,PartnerId=d.PartnerId
                                        };


                                        ViewBag.InventoryByArea = inventory;


            dynamic inventory2 =   from d in _context.Inventorys
                                        join ig in _context.InventoryGroups on  d.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        join pa in _context.Partners on da.PartnerId equals pa.Id
                                        join dr in _context.Drivers on pa.Id equals dr.PartnerId
                                        join us in _context.Users on dr.UserGuid equals Guid.Parse(us.Id)  //grap the user stuff
                                        where pa.Id == thisPartner.Id
                                        
                                        orderby da.Name
                                        select  new 
                                        {
                                        DaPartnerId=da.PartnerId,FirstName= us.FirstName,LastName=us.LastName,DriverGuid=dr.UserGuid ,DaName= da.Name,DaOpenTime=da.OpenTime,DaCloseTime=da.ClosedTime, Label=d.Label,ID=d.ID,Quantity= d.Quantity,Price=d.Price,ImageFilePath=d.ImageFilePath,Status=d.Status,OnHand=d.OnHand,BestBefore=d.BestBefore,OrderDate=d.OrderDate,Cost=d.Cost,Supplier=d.Supplier,Notes=d.Notes,Photo=d.Photo,THCContent=d.THCContent,CBDContent=d.CBDContent,Likes=d.Likes,PricePerGram=d.PricePerGram,PricePerQuarter=d.PricePerQuarter,PricePerhalf=d.PricePerhalf,PricePerOz=d.PricePerOz,CostPerGram=d.CostPerGram,Discount=d.Discount,UPC=d.UPC,Qualities=d.Qualities,catagory=d.catagory,PartnerId=d.PartnerId
                                        };


                                        ViewBag.InventoryByArea2 = inventory2;
            //var inventory = await _context.Inventorys.SingleOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }  
         

            string[] myvar2 = {"Concentrates","Flowers","Edibales","Topicals","Hashish","Oils","Paraphanalia"};

            ViewBag.query = _context.Inventorys.Where(c=> c.Status==1);
            var Products = _context.Inventorys.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Label });

            var mylist2 = myvar2.Select(m => new SelectListItem { Text = m, Value = m });
             
            ViewBag.MyList2=mylist2;

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
