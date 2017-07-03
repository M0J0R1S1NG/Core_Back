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
        {
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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userGuid = Guid.Parse(user.Id);
            var order =  (_context.Orders).Where(m => m.AppUser == userGuid ).OrderByDescending(z=> z.OrderDate);
             var partners = _context.Partners.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.Name });
            ViewBag.partners = new SelectList(partners, "Id", "Value");
             var inventorys = InventoryByAreaVar.OrderBy(c => c.DeliveryAreaName).Select(x => new {Id = x.InventoryId, Value = x.Label + " : " + x.DeliveryAreaName});
            ViewBag.inventorys = new SelectList(inventorys, "Id", "Value");
            return View(order);
        }
        [HttpGet]
        //[ValidateAntiForgeryToken]
        // GET: Orders/Accept
        public async Task<IActionResult> NoShow(int ID,string code, [Bind(" DriverId")] Order order)
        {
                if (ModelState.IsValid)
                {
                     var thisOrder = await _context.Orders.SingleAsync(m => m.ID == ID);

                    if (order == null || code==null || thisOrder.Status==7)
                    {
                        return NotFound();
                    }
                    if (thisOrder.AppUser ==Guid.Parse(code)){
                        thisOrder.Status=7;
                        
                        _context.Update(thisOrder);
                        await _context.SaveChangesAsync();
                        
                        double currBalance=0; 
                         int LatestDriverBalanceID=0;                       
                         LatestDriverBalanceID = _context.DriverBalances.Where(x=> x.Status>0 && x.DriverId==order.DriverId).Max(n => n.ID);
                         if(LatestDriverBalanceID.CompareTo(0)>=0){
                            double latestBalance= _context.DriverBalances.Where(x=>x.ID ==LatestDriverBalanceID).Select(z=> z.RunningBalance ).SingleOrDefault();

                            if(latestBalance.CompareTo(0)>=0){ currBalance=0;}else{currBalance=latestBalance;}
                         }
                        IList<DriverBalance> thisBalance = _context.DriverBalances.Where(z=> z.DriverId== thisOrder.DriverId && z.merchant_transaction_id==thisOrder.GUID).ToList();
                        foreach (var balanceItems in thisBalance){
                            balanceItems.Status=-1;
                            balanceItems.DriverId=order.DriverId;
                            balanceItems.RunningBalance=currBalance-Decimal.ToDouble(thisOrder.Total);
                            _context.Update(balanceItems);
                            
                        }          
                        await _context.SaveChangesAsync();    
                        var debitWay =_context.DebitWay.Where(x=> x.merchant_transaction_id.Contains(thisOrder.GUID.ToString())).Single();   
                        string[] item_codes = debitWay.item_code.Split(',');
                        var itemQ= item_codes[0].Split('x');
                        var quantity= itemQ[0];
                        var InventoryId=itemQ[1];
                        
                        var newtot=0.0;
                        var newcost=0.0;
                        //var LatestDriverBalanceID = _context.DriverBalances.Where(x=> x.DriverId==Int32.Parse("0")).Max(n => n.ID);
                        //var latestBalance= _context.DriverBalances.Where(x=> x.ID ==LatestDriverBalanceID).Select(z=> z.RunningBalance );
                        
                        
                        for (var i=0 ;i<item_codes.Length-1;i++){
                                itemQ = item_codes[i].Split('x');
                                quantity = itemQ[0];
                                InventoryId = itemQ[1];
                                Inventory thisInventory=_context.Inventorys.Where(z=> z.ID==Int32.Parse(InventoryId)).Single();
                                thisInventory.Quantity=thisInventory.Quantity+Int32.Parse(quantity);
                                newcost+=thisInventory.Cost * Int32.Parse(quantity);
                                newtot+=thisInventory.Price * Int32.Parse(quantity);
                                //newBalance.RunningBalance-=thisInventory.Cost;
                                _context.Update(thisInventory);
                        }
                                await _context.SaveChangesAsync();
                                return StatusCode(200);
                    }
                }
                return NotFound();
        }
        public async Task<IActionResult> Delivered(int ID,string code, [Bind(" DriverId")] Order order)
        {
                if (ModelState.IsValid)
                {
                     var thisOrder = await _context.Orders.SingleAsync(m => m.ID == ID);

                    if (order == null || code==null || thisOrder.Status==10)
                    {
                        return NotFound();
                    }
                        if (thisOrder.AppUser ==Guid.Parse(code)){
                            thisOrder.Status=10;
                            
                            _context.Update(thisOrder);
                            await _context.SaveChangesAsync();
                            var LatestDriverBalanceID = _context.DriverBalances.Where(x=> x.Status>0 && x.DriverId==order.DriverId).Max(n => n.ID);
                            var latestBalance= _context.DriverBalances.Where(x=>x.ID ==LatestDriverBalanceID).Select(z=> z.RunningBalance );
                            IList<DriverBalance> thisBalance = _context.DriverBalances.Where(z=> z.DriverId== thisOrder.DriverId && z.merchant_transaction_id==thisOrder.GUID).ToList();
                           foreach (var balanceItems in thisBalance){
                                balanceItems.Status=1;
                                balanceItems.DriverId=order.DriverId;
                                balanceItems.RunningBalance=latestBalance.SingleOrDefault()+Decimal.ToDouble(thisOrder.Total);
                                _context.Update(balanceItems);
                                
                            }
                            await _context.SaveChangesAsync();
                            return StatusCode(200);
                    }
                }
                return NotFound();
        }
        [HttpGet]
        //[ValidateAntiForgeryToken]
        // GET: Orders/Accept
        public async Task<IActionResult> Accept(int ID,string code, [Bind(" DriverId")] Order order)
        {
                if (ModelState.IsValid)
                {
                var thisOrder = await _context.Orders.SingleAsync(m => m.ID == ID);
                IList<DriverBalance> thisDriverBalance =  _context.DriverBalances.Where(m => m.DriverId == 0 && m.merchant_transaction_id==thisOrder.GUID ).ToList();;
                if (order == null || code==null || thisOrder.Status==5 || thisOrder==null || thisDriverBalance==null)
                {
                    return NotFound();
                }
                if (thisOrder.AppUser ==Guid.Parse(code)){

                    thisOrder.Status=5;
                    thisOrder.DeliveryDate=DateTime.Now.AddMinutes(30);
                    thisOrder.DriverId = order.DriverId;

                    _context.Update(thisOrder);
                    await _context.SaveChangesAsync();
                    

                    foreach (var item in thisDriverBalance){
                        item.DriverId=thisOrder.DriverId;
                        item.Status=3;

                        
                    }
                         _context.UpdateRange(thisDriverBalance);
                        await _context.SaveChangesAsync();



                    
                    var thisDriver= await _context.Drivers.SingleAsync(m=> m.ID == order.DriverId );
                    var thisDriverUser= await _context.Users.SingleAsync(m=> Guid.Parse(m.Id) == thisDriver.UserGuid );
                    var user = await _context.Users.SingleAsync(m=>  Guid.Parse(m.Id)== thisOrder.AppUser);

                    string smsmessage="Your order has been dispatched." + (char)10 + (char)13 + "Your delivery person is " + thisDriverUser.FirstName + (char)10 + (char)13 + "You can contact them by phone or text at: " + thisDriver.PhoneNumber  + (char)10 + (char)13+ "Your order is expected to be delivered by " + thisOrder.DeliveryDate.ToLocalTime();
                    string DriverSMS="Order Number:" + thisOrder.ID  + (char)10 + (char)13;
                     DriverSMS+= "Phone Number:" + await _userManager.GetPhoneNumberAsync(user)  + (char)10 + (char)13;
                     DriverSMS+= "Expected Delivery is at:" +  thisOrder.DeliveryDate.Hour + ":" + thisOrder.DeliveryDate.Minute;
                     DriverSMS+= "Click link when delivered:";
                     string acceptUrl="Https://" + Request.Host;
                     string acceptOrderLink =acceptUrl + "/Orders/Delivered?DriverId=" + thisDriver.ID +"&ID=" + thisOrder.ID  ;
                     string NoShowOrderLink =acceptUrl + "/Orders/NoShow?DriverId=" + thisDriver.ID +"&ID=" + thisOrder.ID  ;
                     acceptOrderLink +=  "&code=" + thisOrder.AppUser+ (char)10 + (char)13;
                     NoShowOrderLink += "&code=" + thisOrder.AppUser+ (char)10 + (char)13;
                    DriverSMS+= acceptOrderLink+ (char)10 + (char)13 + NoShowOrderLink;

                    var item_codeSplit=thisOrder.PhoneNumber.Split(',');
                    var OrderCustomerPhone = item_codeSplit[0];
                    

                    await _smsSender.SendSmsAsync(OrderCustomerPhone, smsmessage);
                    await _smsSender.SendSmsAsync(thisDriver.PhoneNumber, DriverSMS);





                    return StatusCode(200);
                }
            }
            return NotFound();
        }
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApi([Bind("Total,GeocodedAddress,Weight,PaymentType,Details,PhoneNumber,SpecialInstructions,Status,DriverId,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
             {   
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                order.AppUser= Guid.Parse(user.Id);
                order.OrderDate = DateTime.Now;
                if (order.GeocodedAddress==null || order.GeocodedAddress==""){order.GeocodedAddress= user.DeliveryAddress;}
                if (order.SpecialInstructions.Contains("Must add")) {order.SpecialInstructions= "";}
                _context.Add(order);

                await _context.SaveChangesAsync();


                string message = "<p>Thank You, we got your order.</p>We are delivering the following: " +  order.Details +  " <br>To: " ;
                message += order.GeocodedAddress + "<br>";
                message+= "Special Instructions:" + order.SpecialInstructions + "<br>";
                //message += "The total for this order is " +  order.Total.ToString() + "<br>";
                message += "<p>Your order number is " + order.ID + "-" + order.AppUser + "</p>";
                
                string smsmessage="Thanks we got your order. We are delivering the following: " +  order.Details + (char)10 + (char)13 + "To: "  ;
                smsmessage += order.GeocodedAddress + (char)10 + (char)13;
                smsmessage+= "Special Instructions:" + order.SpecialInstructions + (char)10 + (char)13;
                //smsmessage += "The total for this order is " +  order.Total.ToString() + (char)10 + (char)13;
                smsmessage += "Your order number is " + order.ID + "-" + order.AppUser ;


                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), smsmessage);
                await _smsSender.SendSmsAsync("6475284350", _userManager.GetPhoneNumberAsync(user).Result + " " + order.GeocodedAddress + " " +  order.Details);
                await _smsSender.SendSmsAsync("4168028129", _userManager.GetPhoneNumberAsync(user).Result + " " + order.GeocodedAddress + " " +  order.Details);
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "New Order", message);
                await _emailSender.SendEmailAsync("andrewmoore46@gmail.com",  _userManager.GetPhoneNumberAsync(user).Result + " " + "New Order", message);
                await _emailSender.SendEmailAsync("moorea@uberduber.com",  _userManager.GetPhoneNumberAsync(user).Result + " " + "New Order", message);
                //var item = new JsonResult( _context.Orders.SingleOrDefaultAsync());
                
                
                
                string[] item_codes = order.PhoneNumber.Split(',');
                for (var i=0 ;i<item_codes.Length-1;i++){
                    var itemQ = item_codes[i].Split('x');
                    var quantity = itemQ[0].Replace("'","");

                    var InventoryId = itemQ[1];
                    Inventory thisInventory=_context.Inventorys.Where(z=> z.ID==Int32.Parse(InventoryId)).Single();
                    thisInventory.Quantity=thisInventory.Quantity-Int32.Parse(quantity);
                    _context.Update(thisInventory);
                    await  _context.SaveChangesAsync();     
                }
                return StatusCode(200);
            }
            
            return StatusCode(404);
        }
        [AllowAnonymous]
        [HttpPost]      
        //[ValidateAntiForgeryToken]  
        public async Task<string> Interac([Bind("Total,PhoneNumber,GeocodedAddress,Weight,PaymentType,Details,SpecialInstructions,Status,DriverId,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
             {   
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                order.AppUser= Guid.Parse(user.Id);
                order.OrderDate = DateTime.Now;
                
                if (order.GeocodedAddress==null || order.GeocodedAddress==""){order.GeocodedAddress= user.StreetNumber +"-"+user.DeliveryAddress;}
                if (order.SpecialInstructions.Contains("Must add")) {order.SpecialInstructions= "";}
                _context.Add(order);
                
                Guid newGuid = Guid.NewGuid();
                order.GUID=newGuid;
                await _context.SaveChangesAsync();
                
                return  newGuid.ToString();
            }
            
            return "error";
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
