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
                //Https://@Context.Request.Host/DebitWay/Create
  
                string acceptUrl="";
                
                //int strlength =debitWay.return_url.Length;
                    //as long as return url is same legth as DebitWay/Create or 15
                    //Https://" + Request.Host  + "/DebitWay/Create";
                //acceptUrl= debitWay.return_url.Substring(0,strlength-15);
                acceptUrl="Https://" + Request.Host;
                debitWay.transaction_date_datetime=DateTime.Now;
                if (debitWay.status=="cash"){
                    debitWay.transaction_result="success";
                    debitWay.transaction_type=debitWay.status;
                }else if (debitWay.status=="paypal"){
                     debitWay.transaction_result="success";
                     debitWay.transaction_type=debitWay.status;
                }
                 TempData["transaction_result"]=debitWay.transaction_result;
                 TempData["error"]=debitWay.customer_errors_meaning;
                _context.Add(debitWay);
                await _context.SaveChangesAsync();
                
                
                if (debitWay.transaction_result!="success"){
                   return RedirectToAction("forsale","Inventorys");
                   
                }
                string[] customString = debitWay.custom.Split(',');
               customString.ToList().Add("hjsdkah");                
               int numvars=customString.Length;
                int numDrivers = (numvars-3)/2;

                string userId=customString[0].Trim();
                string SpecialInstructions=customString[1];
                string newPartnerId=customString[2];
               string[]  thisOrdersSMS = new string[numDrivers];
                string[]  thisOrdersDriverIds = new string[numDrivers]; 
               List<string> newSMSNumbers;
               List<string> newDriversIds;
               
               if (numDrivers>1){
                    for (var j=0;j <=numDrivers-1; j++ ){
                        newSMSNumbers=customString[j+3].Split(':').ToList();
                        thisOrdersSMS.Append(newSMSNumbers[1]);
                        thisOrdersSMS[j]=newSMSNumbers[1];
                    }
                    for (var j=0;j <=numDrivers-1; j++ ){
                        newDriversIds=customString[j+3+numDrivers].Split(':').ToList();
                        thisOrdersDriverIds.Append(newDriversIds[1]);
                        thisOrdersDriverIds[j]=newDriversIds[1];
                    }
                    string newSMSNumber=thisOrdersSMS[0];
               }
                
                
               


                Partner  areaPartner=await _context.Partners.Where(x=> x.Id==Int32.Parse(newPartnerId)).SingleAsync();
                IQueryable<Driver> areaDrivers =  _context.Drivers.Where(x=> x.PartnerId==Int32.Parse(newPartnerId)).OrderBy(x=> x.Status).Select(x => new Driver {EmailAddress=x.EmailAddress,PhoneNumber=x.PhoneNumber,ID=x.ID });
                
             
                
                
                
                Order thisOrder = _context.Orders.Where(o=> o.GUID == Guid.Parse(debitWay.merchant_transaction_id)).First();
                var user = await _userManager.FindByIdAsync(userId);
                thisOrder.DeliveryDate = DateTime.Now.AddMinutes(30);
                // if (thisOrder.GeocodedAddress==null || thisOrder.GeocodedAddress==""){thisOrder.GeocodedAddress= user.DeliveryAddress;}
                // if (thisOrder.SpecialInstructions.Contains("Must add")) {thisOrder.SpecialInstructions= "";}
                thisOrder.Status=2;
                thisOrder.CustomerId = Int32.Parse(newPartnerId);//put partnerId in here
                //thisOrder.DriverId=Int32.Parse(thisDriver);//fill in 
                thisOrder.PhoneNumber=await _userManager.GetPhoneNumberAsync(user); //+ "," +  newSMSNumber;
                var thisOrderDeliveryFlatFee=thisOrder.Weight;
                _context.Update(thisOrder);
                await _context.SaveChangesAsync();

                string message = "<p>Thank You, we got your order.</p>We are delivering the following: " +  thisOrder.Details +  " <br>To: " ;
                message += thisOrder.GeocodedAddress + "<br>";
                message+= "Special Instructions:" + thisOrder.SpecialInstructions + "<br>";
                message += "<p>Your order number is " + thisOrder.ID + "-" + thisOrder.AppUser + "</p>";
                
                string smsmessage="Thanks we got your order. We are delivering the following: " +  thisOrder.Details + (char)10 + (char)13 + "To: "  ;
                smsmessage += thisOrder.GeocodedAddress + (char)10 + (char)13;
                smsmessage+= "Special Instructions:" + thisOrder.SpecialInstructions + (char)10 + (char)13;
                smsmessage += "Your order number is " + thisOrder.ID + "-" + thisOrder.AppUser ;
                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), smsmessage);
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "New Order", message);
                             
                await _smsSender.SendSmsAsync(areaPartner.SMSNumber,acceptUrl + " " +  _userManager.GetPhoneNumberAsync(user).Result + " " + thisOrder.GeocodedAddress + " " +  thisOrder.Details);
                await _emailSender.SendEmailAsync(areaPartner.EmailAddress,  _userManager.GetPhoneNumberAsync(user).Result + " " + "New Order",   message);
              

                foreach (var myDrivers in areaDrivers){
                    string acceptOrderLink =acceptUrl + "/Orders/Accept?DriverId=" +myDrivers.ID+"&ID=" + thisOrder.ID  ;
                     acceptOrderLink +=  "&code=" + thisOrder.AppUser;
                    string driverSMS =acceptOrderLink+"  " + _userManager.GetPhoneNumberAsync(user).Result + " " + thisOrder.GeocodedAddress + " " +  thisOrder.Details;
                     
                    
                    await _smsSender.SendSmsAsync(myDrivers.PhoneNumber, driverSMS);
                    await _emailSender.SendEmailAsync(myDrivers.EmailAddress,  _userManager.GetPhoneNumberAsync(user).Result + " " + "New Order", driverSMS);
                 }
                //await _smsSender.SendSmsAsync("4168028129", _userManager.GetPhoneNumberAsync(user).Result + " " + thisOrder.GeocodedAddress + " " +  thisOrder.Details);
                
                //await _emailSender.SendEmailAsync("andrewmoore46@gmail.com",  _userManager.GetPhoneNumberAsync(user).Result + " " + "New Order", message);
                //await _emailSender.SendEmailAsync("moorea@uberduber.com",  _userManager.GetPhoneNumberAsync(user).Result + " " + "New Order", message);
                //var item = new JsonResult( _context.Orders.SingleOrDefaultAsync());


                //deduct from inventory
                string[] item_codes = debitWay.item_code.Split(',');
                    var itemQ= item_codes[0].Split('x');;
                    var quantity= itemQ[0];
                    var InventoryId=itemQ[1];
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    var newbal=0;
                    var newtot=0.0;
                    var newcost=0.0;
                    //var LatestDriverBalanceID = _context.DriverBalances.Where(x=> x.DriverId==Int32.Parse("0")).Max(n => n.ID);
                    //var latestBalance= _context.DriverBalances.Where(x=> x.ID ==LatestDriverBalanceID).Select(z=> z.RunningBalance );
                    
                    DriverBalance newBalance = new DriverBalance();
                    for (var i=0 ;i<item_codes.Length-1;i++){
                            itemQ = item_codes[i].Split('x');
                            quantity = itemQ[0];
                            InventoryId = itemQ[1];
                            newBalance.DeliveryFeeCustomer=0;
                            newBalance.DeliveryFeeSupplier=0;
                            newBalance.DriverFlatRate=0;
                            newBalance.DriverPercentageRate=0;
                            newBalance.DeliveryDate=DateTime.Now;
                            newBalance.CreatedDate =DateTime.Now;
                            //newBalance.DriverId=Int32.Parse(thisDriver);
                            newBalance.DriverId=0;
                            newBalance.PartnerId=Int32.Parse(newPartnerId);
                            newBalance.quantity=Int32.Parse(quantity);
                            newBalance.InventoryId=Int32.Parse(InventoryId);
                            newBalance.merchant_transaction_id=Guid.Parse(debitWay.merchant_transaction_id);
                            //Status pending initial insert must change t valid status after delivery confirmed
                            newBalance.Status=0;
                            newBalance.CreditOrDebit=false;
                            newBalance.CustomerGuid=Guid.Parse(userId);   
                            Inventory thisInventory=_context.Inventorys.Where(z=> z.ID==Int32.Parse(InventoryId)).Single();
                            thisInventory.Quantity=thisInventory.Quantity-Int32.Parse(quantity);
                            newcost+=thisInventory.Cost * Int32.Parse(quantity);
                            newtot+=thisInventory.Price * Int32.Parse(quantity);
                            //newBalance.RunningBalance-=thisInventory.Cost;
                            newBalance.NetAmount=newcost;
                            newBalance.TotalAmount=newtot;
                            newBalance.TransactionType=4;
                            newBalance.DeliveryFeeCustomer=Decimal.ToSingle(thisOrderDeliveryFlatFee);
                            newBalance.Taxes=debitWay.amount - newtot -Decimal.ToSingle(thisOrderDeliveryFlatFee);
                            newBalance.LastChangeBy=userId;
                            newBalance.LastChangeDate=DateTime.Now;
                            _context.Update(thisInventory);
                            await _context.AddAsync(newBalance);
                            await  _context.SaveChangesAsync();     
                }
               
                


                if (debitWay.transaction_result!="success"){
                    ViewData["message"]="Failed Transaction";

                    return RedirectToAction("forsale","Inventorys");

                }
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
