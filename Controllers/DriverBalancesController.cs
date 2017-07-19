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
using Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Helpers;
using Core;
using Core.Models.AccountViewModels;


namespace Core.Controllers
{
     [Authorize(Roles="Admin")]
    public class DriverBalancesController : Controller
    { private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public DriverBalancesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
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

        [Authorize]
        // GET: DriverBalances
        public async Task<IActionResult> Index()
        { 
            string  uid=_userManager.GetUserId(User);

           //
            if (uid.Length>10){
            // int thisDriverId=(from d in _context.Drivers 
            //                  where d.UserGuid==Guid.Parse(uid)
                            
            //                  select d.ID).Single();
            var ThisDriverBalances=await _context.DriverBalances.ToListAsync();
            
            var  driverids =   from d in _context.Drivers
                                        join us in _context.Users on d.UserGuid equals    Guid.Parse(us.Id) 
                                        join pa in _context.Partners on d.PartnerId equals pa.Id     // first join
                                                
                                        orderby us.LastName
                                        select  new 
                                        {
                                           Id=d.ID,Value=us.FirstName + " Partner: " +  pa.Name
                                        };
            ViewBag.drivers = new SelectList(driverids, "Id", "Value");

            IQueryable<InventoryByArea>  InventoryByAreaVar = from i in _context.Inventorys
                                        join ig in _context.InventoryGroups on  i.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        //join us in _context.Users on ig.DeliveryAreaId equals us.DeliveryAreaId
                                        //where da.ID == UserAreaId.DeliveryAreaId
                                        //where d.Quantity > 0
                                        orderby da.Name
                                        select  new InventoryByArea
                                        {
                                           DeliveryAreaName=da.Name,Label=i.Label, InventoryCatagory=i.catagory, InventoryDescription=i.Description,DeliveryAreaID=da.ID,Quantity= i.Quantity,Price=i.Price,ImageFilePath=i.ImageFilePath, InventoryId=i.ID
                                        };
            var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.Name).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Email });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var partners = _context.Partners.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Name });
            ViewBag.partners = new SelectList(partners, "Id", "Value");
             var inventorys = InventoryByAreaVar.OrderBy(c => c.DeliveryAreaName).Select(x => new {Id = x.InventoryId, Value = x.Label + " : " + x.DeliveryAreaName});
            ViewBag.inventorys = new SelectList(inventorys, "Id", "Value");
            
            
            return View(ThisDriverBalances);
            }
          return View(_context.DriverBalances.ToList());
        }

        // GET: DriverBalances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverBalance = await _context.DriverBalances
                .SingleOrDefaultAsync(m => m.ID == id);
            if (driverBalance == null)
            {
                return NotFound();
            }
             ViewBag.UserId=_userManager.GetUserId(User);
          
                        var  driverids =   from d in _context.Drivers
                                        join us in _context.Users on d.UserGuid equals    Guid.Parse(us.Id) 
                                        join pa in _context.Partners on d.PartnerId equals pa.Id     // first join
                                                
                                        orderby us.LastName
                                        select  new 
                                        {
                                           Id=d.ID,Value=us.FirstName + " Partner: " +  pa.Name
                                        };


                                        ViewBag.drivers = new SelectList(driverids, "Id", "Value");


             IQueryable<InventoryByArea>  InventoryByAreaVar = from i in _context.Inventorys
                                        join ig in _context.InventoryGroups on  i.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        //join us in _context.Users on ig.DeliveryAreaId equals us.DeliveryAreaId
                                        
                                        //where da.ID == UserAreaId.DeliveryAreaId
                                        //where d.Quantity > 0
                                        orderby da.Name
                                        select  new InventoryByArea
                                        {
                                           DeliveryAreaName=da.Name,Label=i.Label, InventoryCatagory=i.catagory, InventoryDescription=i.Description,DeliveryAreaID=da.ID,Quantity= i.Quantity,Price=i.Price,ImageFilePath=i.ImageFilePath, InventoryId=i.ID
                                        };


                                        


           var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.Name).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
                        
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Email });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var partners = _context.Partners.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Name });
            ViewBag.partners = new SelectList(partners, "Id", "Value");
             var inventorys = InventoryByAreaVar.OrderBy(c => c.DeliveryAreaName).Select(x => new {Id = x.InventoryId, Value = x.Label + " : " + x.DeliveryAreaName});
            ViewBag.inventorys = new SelectList(inventorys, "Id", "Value");
            return View(driverBalance);
        }

        // GET: DriverBalances/Create
        public IActionResult Create()
        {               ViewBag.UserId=_userManager.GetUserId(User);
          
                        var  driverids =   from d in _context.Drivers
                                        join us in _context.Users on d.UserGuid equals    Guid.Parse(us.Id) 
                                        join pa in _context.Partners on d.PartnerId equals pa.Id     // first join
                                                
                                        orderby us.LastName
                                        select  new 
                                        {
                                           Id=d.ID,Value=us.FirstName + " Partner: " +  pa.Name
                                        };


                                        ViewBag.drivers = new SelectList(driverids, "Id", "Value");


             IQueryable<InventoryByArea>  InventoryByAreaVar = from i in _context.Inventorys
                                        join ig in _context.InventoryGroups on  i.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        //join us in _context.Users on ig.DeliveryAreaId equals us.DeliveryAreaId
                                        
                                        //where da.ID == UserAreaId.DeliveryAreaId
                                        //where d.Quantity > 0
                                        orderby da.Name
                                        select  new InventoryByArea
                                        {
                                           DeliveryAreaName=da.Name,Label=i.Label, InventoryCatagory=i.catagory, InventoryDescription=i.Description,DeliveryAreaID=da.ID,Quantity= i.Quantity,Price=i.Price,ImageFilePath=i.ImageFilePath, InventoryId=i.ID
                                        };


                                        


           var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.Name).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
                        
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Email });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var partners = _context.Partners.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Name });
            ViewBag.partners = new SelectList(partners, "Id", "Value");
             var inventorys = InventoryByAreaVar.OrderBy(c => c.DeliveryAreaName).Select(x => new {Id = x.InventoryId, Value = x.Label + " : " + x.DeliveryAreaName});
            ViewBag.inventorys = new SelectList(inventorys, "Id", "Value");

            return View();
        }

        // POST: DriverBalances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,merchant_transaction_id,DriverId,InventoryId,DriverPercentageRate,DriverFlatRate,quantity,Taxes,TotalAmount,NetAmount,RunningBalance,DeliveryFeeCustomer,DeliveryFeeSupplier,Status,CreditOrDebit,Notes,TransactionType,PartnerId,CustomerId,DeliveryDate,CreatedDate,LastChangeDate,CreateBy,LastChangeBy")] DriverBalance driverBalance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driverBalance);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(driverBalance);
        }



        // GET: DriverBalances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverBalance = await _context.DriverBalances.SingleOrDefaultAsync(m => m.ID == id);
            if (driverBalance == null)
            {
                return NotFound();
            }
             ViewBag.UserId=_userManager.GetUserId(User);
          
                        var  driverids =   from d in _context.Drivers
                                        join us in _context.Users on d.UserGuid equals    Guid.Parse(us.Id) 
                                        join pa in _context.Partners on d.PartnerId equals pa.Id     // first join
                                                
                                        orderby us.LastName
                                        select  new 
                                        {
                                           Id=d.ID,Value=us.FirstName + " Partner: " +  pa.Name
                                        };


                                        ViewBag.drivers = new SelectList(driverids, "Id", "Value");


             IQueryable<InventoryByArea>  InventoryByAreaVar = from i in _context.Inventorys
                                        join ig in _context.InventoryGroups on  i.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        //join us in _context.Users on ig.DeliveryAreaId equals us.DeliveryAreaId
                                        
                                        //where da.ID == UserAreaId.DeliveryAreaId
                                        //where d.Quantity > 0
                                        orderby da.Name
                                        select  new InventoryByArea
                                        {
                                           DeliveryAreaName=da.Name,Label=i.Label, InventoryCatagory=i.catagory, InventoryDescription=i.Description,DeliveryAreaID=da.ID,Quantity= i.Quantity,Price=i.Price,ImageFilePath=i.ImageFilePath, InventoryId=i.ID
                                        };


                                        


           var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.Name).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
                        
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Email });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var partners = _context.Partners.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Name });
            ViewBag.partners = new SelectList(partners, "Id", "Value");
             var inventorys = InventoryByAreaVar.OrderBy(c => c.DeliveryAreaName).Select(x => new {Id = x.InventoryId, Value = x.Label + " : " + x.DeliveryAreaName});
            ViewBag.inventorys = new SelectList(inventorys, "Id", "Value");


            return View(driverBalance);
        }

        // POST: DriverBalances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,merchant_transaction_id,DriverId,InventoryId,DriverPercentageRate,DriverFlatRate,quantity,Taxes,TotalAmount,NetAmount,RunningBalance,DeliveryFeeCustomer,DeliveryFeeSupplier,Status,CreditOrDebit,Notes,TransactionType,PartnerId,CustomerId,DeliveryDate,CreatedDate,LastChangeDate,CreateBy,LastChangeBy")] DriverBalance driverBalance)
        {
            if (id != driverBalance.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driverBalance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverBalanceExists(driverBalance.ID))
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
            return View(driverBalance);
        }

        // GET: DriverBalances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverBalance = await _context.DriverBalances
                .SingleOrDefaultAsync(m => m.ID == id);
            if (driverBalance == null)
            {
                return NotFound();
            }
             ViewBag.UserId=_userManager.GetUserId(User);
          
                        var  driverids =   from d in _context.Drivers
                                        join us in _context.Users on d.UserGuid equals    Guid.Parse(us.Id) 
                                        join pa in _context.Partners on d.PartnerId equals pa.Id     // first join
                                                
                                        orderby us.LastName
                                        select  new 
                                        {
                                           Id=d.ID,Value=us.FirstName + " Partner: " +  pa.Name
                                        };


                                        ViewBag.drivers = new SelectList(driverids, "Id", "Value");


             IQueryable<InventoryByArea>  InventoryByAreaVar = from i in _context.Inventorys
                                        join ig in _context.InventoryGroups on  i.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        //join us in _context.Users on ig.DeliveryAreaId equals us.DeliveryAreaId
                                        
                                        //where da.ID == UserAreaId.DeliveryAreaId
                                        //where d.Quantity > 0
                                        orderby da.Name
                                        select  new InventoryByArea
                                        {
                                           DeliveryAreaName=da.Name,Label=i.Label, InventoryCatagory=i.catagory, InventoryDescription=i.Description,DeliveryAreaID=da.ID,Quantity= i.Quantity,Price=i.Price,ImageFilePath=i.ImageFilePath, InventoryId=i.ID
                                        };


                                        


           var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.Name).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
                        
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Email });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var partners = _context.Partners.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Name });
            ViewBag.partners = new SelectList(partners, "Id", "Value");
             var inventorys = InventoryByAreaVar.OrderBy(c => c.DeliveryAreaName).Select(x => new {Id = x.InventoryId, Value = x.Label + " : " + x.DeliveryAreaName});
            ViewBag.inventorys = new SelectList(inventorys, "Id", "Value");
            return View(driverBalance);
        }

        // POST: DriverBalances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driverBalance = await _context.DriverBalances.SingleOrDefaultAsync(m => m.ID == id);
            _context.DriverBalances.Remove(driverBalance);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DriverBalanceExists(int id)
        {
            return _context.DriverBalances.Any(e => e.ID == id);
        }
         public IActionResult LoadDriver()
        {               ViewBag.UserId=_userManager.GetUserId(User);
          
                        var  driverids =   from d in _context.Drivers
                                        join us in _context.Users on d.UserGuid equals    Guid.Parse(us.Id) 
                                        join pa in _context.Partners on d.PartnerId equals pa.Id     // first join
                                                
                                        orderby us.LastName
                                        select  new 
                                        {
                                           Id=d.ID,Value=us.FirstName + " Partner: " +  pa.Name
                                        };


                                        ViewBag.drivers = new SelectList(driverids, "Id", "Value");


             IQueryable<InventoryByArea>  InventoryByAreaVar = from i in _context.Inventorys
                                        join ig in _context.InventoryGroups on  i.ID equals    ig.InventoryId      // first join
                                        join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                        //join us in _context.Users on ig.DeliveryAreaId equals us.DeliveryAreaId
                                        
                                        //where da.ID == UserAreaId.DeliveryAreaId
                                        //where d.Quantity > 0
                                        orderby da.Name
                                        select  new InventoryByArea
                                        {
                                           DeliveryAreaName=da.Name,Label=i.Label, InventoryCatagory=i.catagory, InventoryDescription=i.Description,DeliveryAreaID=da.ID,Quantity= i.Quantity,Price=i.Price,ImageFilePath=i.ImageFilePath, InventoryId=i.ID
                                        };


                                        


           var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.Name).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
                        
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Email });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var partners = _context.Partners.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Name });
            ViewBag.partners = new SelectList(partners, "Id", "Value");
             var inventorys = InventoryByAreaVar.OrderBy(c => c.DeliveryAreaName).Select(x => new {Id = x.InventoryId, Value = x.Label + " : " + x.DeliveryAreaName});
            ViewBag.inventorys = new SelectList(inventorys, "Id", "Value");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadDriver([Bind("ID,merchant_transaction_id,DriverId,InventoryId,DriverPercentageRate,DriverFlatRate,quantity,Taxes,TotalAmount,NetAmount,RunningBalance,DeliveryFeeCustomer,DeliveryFeeSupplier,Status,CreditOrDebit,Notes,TransactionType,PartnerId,CustomerId,DeliveryDate,CreatedDate,LastChangeDate,CreateBy,LastChangeBy")] DriverBalance driverBalance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driverBalance);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(driverBalance);
        }
    }
}
