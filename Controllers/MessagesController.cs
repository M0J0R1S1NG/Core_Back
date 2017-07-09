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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Twilio;
using Twilio.TwiML;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Net.Http;
using Core.Helpers.BingMapTypes;
using Core.Controllers;
using System.Text.Encodings;


namespace Core.Controllers
{
  
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        //private readonly IGeocoder _geoCoder;
        public MessagesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
            //IGeocoder geoCoder)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            //_geoCoder = geoCoder;

        }
        // GET: Partners

       public async Task<string> SMS_Text_Reply(string From ,string Body ,string AccountSid,string FromCity,
                                                string FromCountry,string FromState,string FromZip,string ToCity,
                                                string ToCountry,string ToState,string ToZip,string SmsStatus,
                                                string To,string SMsMessageSid,string SmsSid,string ApiVersion)
        {


       

            var vaout="";
            var urlString = "";
            var orerItemStr = "";
            string buildStr = "";
            
            
            string[] vars;
            if (Body.ToUpper().Contains("CONFIRM") ){
                vars= Body.Split(' ');
                if (vars.Length<=0){
                   
                }
                int orderId=Int32.Parse(vars[1]);
               
                Order thisOrder=_context.Orders.Where(x=> x.ID==orderId && x.Status<0).Single();
                ApplicationUser thisUser = _context.Users.Where(x=> x.Id==thisOrder.AppUser.ToString()).Single();

                if (From.Contains(thisUser.PhoneNumber)==false){
                    vaout = "";
                    vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                    vaout += "<Response>";
                    vaout += "<Message>";
                    vaout += "Sorry the order users phone number :"+ thisUser.PhoneNumber  +" Does not match your number: " + From + ". You can only confirm your own orders you make from your own phone";
                    vaout += "</Message>";
                    vaout += "</Response>";
                    Response.ContentType="text/xml";
                    return  vaout;
                }
                var driversbyAreaPartner =
	
                    from ed in _context.DeliveryAreas
                    
                    join p in  _context.Partners on ed.PartnerId equals p.Id
                    join b in  _context.Drivers on p.Id equals  b.PartnerId
                    join u in  _context.ApplicationUser on b.UserGuid.ToString() equals u.Id
                    //where u.FirstName=="AndrewUser" 
                    where ed.ID==thisUser.DeliveryAreaId
                    select new
                    {
                        Driver=ed.ID,
                        DeliveryArea=ed.Name,
                        DriverId=b.ID,
                        drivername =u.FirstName,
                        //drivername = (from x in Core_Users join z in Drivers on x.Id equals z.UserGuid.ToString() where z.ID == b.ID select new {drivername = x.FirstName}),
                        DriverNumber=b.PhoneNumber,
                        PartnerNumber=p.PhoneNumber,
                        PartnerSMS=p.SMSNumber,
                        p.Name,
                        p.Id
                        
                    };

                    int d=0;
                    string DriverIdStr="";
                    string SMSStr="";
                    int thisPartnerId=-1;
                     var DeliveryAreaName="";
                    foreach (var thisDriver in driversbyAreaPartner) {
                         d+=1;
                              DriverIdStr+="DriverId"+d+":"+thisDriver.DriverId+",";
                              SMSStr+="SMS"+d+":"+thisDriver.DriverNumber+",";
                              thisPartnerId=thisDriver.Id;
                              DeliveryAreaName=thisDriver.DeliveryArea;
                    }
                        string SpecialInstructionsStr=thisUser.StreetName  + thisUser.UnitNumber;
                            DriverIdStr=DriverIdStr.TrimEnd(',');
                            SMSStr= SMSStr.TrimEnd(',');   
                            string customStr=thisUser.Id + ","+ SpecialInstructionsStr+"," + thisPartnerId + "," + SMSStr + "," + DriverIdStr;

                if (thisOrder==null){
                     vaout = "";
                    vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                    vaout += "<Response>";
                    vaout += "<Message>";
                    vaout += "Sorry we couldnt find order: "+orderId +" in the system. Please try again. Enter your text reply exactly like this 'Confirm code#'.  If you cant find your confirmation code. Restart by texting 'order'";
                    vaout += "</Message>";
                    vaout += "</Response>";
                    Response.ContentType="text/xml";
                    return  vaout;
                }
                        string identifier="";
                        string website_unique_id="";
                        string return_url="";
                        string transaction_date=DateTime.Now.ToString();
                        string language="en";
                        string first_name=thisUser.FirstName;
                        string last_name=thisUser.LastName;
                        string address=thisUser.StreetName;
                        string city=thisUser.City;
                        string email=thisUser.Email;
                        string state_or_province=thisUser.Province;
                        string zip_or_postal_code=thisUser.PostalCode;
                        string phone=thisUser.PhoneNumber;
                        string shipping_address=thisUser.DeliveryAddress;
                        string shipping_city=thisUser.City;
                        string shipping_state_or_province=thisUser.Province;
                        string shipping_zip_or_postal_code=thisUser.PostalCode;
                        string shipping_country=thisUser.Country;
                        string item_name=thisOrder.Details;
                        string amount=thisOrder.Total.ToString();
                        string quantity="0";
                        string item_code=thisOrder.PhoneNumber;
                        string custom=customStr;
                        string shipment="yes";
                        string merchant_transaction_id=thisOrder.GUID.ToString();                          
                        string status="sms";    //cash interca credit paypal SMSCash=5
                        string time_stamp=DateTime.Now.ToString();
                        

 string buildStr3="";                    
                    buildStr3 += "identifier=" +  identifier ;
                    buildStr3 += "&website_unique_id=" +  website_unique_id ;
                    buildStr3 += "&return_url=" +  return_url ;
                    buildStr3 += "&transaction_date=" +  transaction_date ;
                    buildStr3 += "&language=" +  language ;
                    buildStr3 += "&first_name=" +  first_name ;
                    buildStr3 += "&last_name=" +  last_name ;
                    buildStr3 += "&address=" +  address ;
                    buildStr3 += "&city=" +  city ;
                    buildStr3 += "&state_or_province=" +  state_or_province ;
                    buildStr3 += "&zip_or_postal_code=" +  zip_or_postal_code ;
                    buildStr3 += "&shipping_state_or_province=" +  email ;
                    buildStr3 += "&phone=" +  phone ;
                    buildStr3 += "&shipping_address=" +  shipping_address ;
                    buildStr3 += "&shipping_city=" +  shipping_city ;
                    buildStr3 += "&shipping_state_or_province=" +  shipping_state_or_province ;
                    buildStr3 += "&shipping_zip_or_postal_code=" +  shipping_zip_or_postal_code ;
                    buildStr3 += "&shipping_country=" +  shipping_country ;
                    buildStr3 += "&item_name=" +  item_name ;
                    buildStr3 += "&amount=" +  amount ;
                    buildStr3 += "&quantity=" +  quantity ;
                    buildStr3 += "&item_code=" +  item_code ;
                    buildStr3 += "&custom=" +  custom ;
                    buildStr3 += "&shipment=" +  shipment ;
                    buildStr3 += "&merchant_transaction_id=" +  merchant_transaction_id ;                          
                    buildStr3 += "&status=" +  status ;
                    buildStr3 += "&time_stamp=" +  time_stamp ;








                 using (var client = new HttpClient())
                {
                    try
                    {
                        string hostStr="";
                        var geocodeRequest="";
                        if (Environment.GetEnvironmentVariable("DEVHOST")==null){
                            hostStr="Https://" + Request.Host;
                        }else{
                            hostStr="Http://" + Request.Host;
                        }
                             


                        geocodeRequest = hostStr + "/REST/v1/Locations?query=";
                        //HttpContent content;
                        //Response.ContentType= "application/x-www-form-urlencoded";
                        
                        //var stringContent = new StringContent(JsonConvert.SerializeObject(DebitWayVars));
                        var getStr=Uri.EscapeUriString($"{hostStr}/DebitWay/Create?{buildStr3}");
                        var response = await client.GetAsync(getStr);
                        response.EnsureSuccessStatusCode();
                        var stringResult = await response.Content.ReadAsStringAsync();
                        
                        //var  retLocation = JsonConvert.DeserializeObject<DebitWay>(stringResult);
                        vaout = "";
                        vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                        vaout += "<Response>";
                        vaout += "<Message>";
                        vaout += "Thank You " + thisUser.FirstName + ". Your order is being processed.  You will be notified when your local delivery area: " + DeliveryAreaName + " gets the order and again when its been dispatched to a driver.";
                        vaout += "</Message>";
                        vaout += "</Response>";
                        Response.ContentType="text/xml";
                        return  vaout;

                         
                 } 
                    catch (HttpRequestException httpRequestException)
                    {
                        return httpRequestException.Message ;
                    }
                }
            }
            
            
            
            
            if (Body.ToUpper().Contains("ADDRESS:") ){
            vars = Body.Split(':');
            if (vars.Length<=1){
                vaout = "";
                vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                vaout += "<Response>";
                vaout += "<Message>";
                vaout += "Sorry we didnt get an address to chceck";
                vaout += "</Message>";
                vaout += "</Response>";
                Response.ContentType="text/xml";
                return  vaout;

            }
            string DeliveryAddress=vars[1];
            using (var client = new HttpClient())
                {
                    try
                    {
                            var geocodeRequest="";
                            geocodeRequest = "https://dev.virtualearth.net/REST/v1/Locations?query=";
                            geocodeRequest+=DeliveryAddress;
                            //var identityTypes='&includeEntityTypes=Address,Neighborhood,PopulatedPlace,Postcode1,AdminDivision1,AdminDivision2,CountryRegion';
                            //geocodeRequest+= identityTypes ;
                            geocodeRequest+= "&includeNeighborhood=true";
                            geocodeRequest+= "&jsonp=GeocodeCallbackGeo&key=AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA";
                           // CallRestService( encodeURI(geocodeRequest));


                           
                        client.BaseAddress = new Uri("https://dev.virtualearth.net");
                        var response = await client.GetAsync($"/REST/v1/Locations?Query={DeliveryAddress}&key=AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA&includeNeighborhood=true");
                        response.EnsureSuccessStatusCode();
                        var stringResult = await response.Content.ReadAsStringAsync();
                        var  retLocation = JsonConvert.DeserializeObject<Core.Helpers.BingMapTypes.RootObject>(stringResult);
                         Response.ContentType="text/plain";

                           var   addressName=retLocation.resourceSets[0].resources[0].name;
                           var   addressLat= retLocation.resourceSets[0].resources[0].point.coordinates[0];
                           var   addressLon=  retLocation.resourceSets[0].resources[0].point.coordinates[1];
                           var   formattedAddress= retLocation.resourceSets[0].resources[0].address.formattedAddress;
                           var   postalCode=  retLocation.resourceSets[0].resources[0].address.postalCode;
                           var   adminDistrict =retLocation.resourceSets[0].resources[0].address.adminDistrict;
                           var   adminDistrict2 = retLocation.resourceSets[0].resources[0].address.adminDistrict2;
                           var   countryRegion = retLocation.resourceSets[0].resources[0].address.countryRegion;
                           var   locality = retLocation.resourceSets[0].resources[0].address.locality;
                           var   neighbourhood = retLocation.resourceSets[0].resources[0].entityType[0];
                           var   addressLine =retLocation.resourceSets[0].resources[0].address.addressLine;
                               
                            


                           List<DeliveryArea> DeliveryAreas = _context.DeliveryAreas.Where(x=> x.Status>=0).ToList();
                            Points dumbPoint =new Points();
                            List<Points> newPoints = new List<Points>{
                                dumbPoint
                            };
                            newPoints.RemoveAt(0);
                            
                            double valLat=0,valLon=0;
                            char[]  delimiters = {')',','};
                            int oddeven=0;
                           foreach(var thisArea in DeliveryAreas){
                              var  rectPoints=thisArea.Points.Replace("new Microsoft.Maps.Location", "").Split(delimiters);  
                                foreach (var str in rectPoints){
                                    
                                    if (str!=""){
                                        if (oddeven==0){
                                            valLat=  Convert.ToDouble(str.Replace(" ","").Replace("(",""));
                                            oddeven=1;
                                        }else{
                                            valLon=  Convert.ToDouble(str.Replace(" ","").Replace("(",""));
                                            Points thisPoint =new Points();
                                            thisPoint.latitude = valLat;
                                            thisPoint.longitude =valLon;
                                            newPoints.Add(thisPoint);
                                            oddeven=0;

                                        }
                                    }
                                }
                                
                                
                                var headstr="";
                                bool InDeliveryArea= pointInPolygon(newPoints,addressLat ,addressLon);
                                newPoints.RemoveRange(0,newPoints.Count());
                                    if(InDeliveryArea ){ 
                                        headstr = $"All set you are in a delivery area "+ (char)10;
                                    }else{
                                        headstr= $"Sorry you are not in a delivery area."+ (char)10;
                                     }
                                    vaout = "";
                                    vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                                    vaout += "<Response>";
                                    vaout += "<Message>";
                                    vaout+=headstr+(char)10+(char)10;
                                    vaout += $"Lat:"+ addressLat + (char)10;
                                    vaout += $"Lon:"+ addressLon + (char)10 +(char)10;
                                    vaout += $"Address:"+ formattedAddress + (char)10;
                                    // vaout += $"AddressName:"+ addressName + (char)10;
                                    // vaout += $"addressLat:"+ addressLat + (char)10;
                                    // vaout += $"addressLon:"+ addressLon + (char)10;
                                    // vaout += $"formattedAddress:"+ formattedAddress + (char)10;
                                    // vaout += $"postalCode:"+ postalCode + (char)10;
                                    // vaout += $"adminDistrict:"+ adminDistrict + (char)10;
                                    // vaout += $"adminDistrict2:"+ adminDistrict2 + (char)10;
                                    // vaout += $"countryRegion:"+ countryRegion + (char)10;
                                    // vaout += $"locality:"+ locality + (char)10;
                                    // vaout += $"neighbourhood:"+ neighbourhood + (char)10;
                                    // vaout += $"addressLine:"+ addressLine + (char)10;
                        //vaout+="<Redirect>";
                        // urlString+= "SmsOut?From='" + From + "'";
                        // urlString+= "&To='" + From + "'";
                        // urlString+= "&Body='" + Body + "'";
                        // urlString+= "&AccountSid='" + AccountSid + "'";
                        // urlString+= "&FromCity='" + FromCity + "'";
                        // urlString+= "&FromCountry='" + FromCountry + "'";
                        // urlString+= "&FromState='" + FromState + "'";
                        // urlString+= "&FromZip='" + FromZip + "'";
                        // urlString+= "&ToCity='" + ToCity + "'";
                        // urlString+= "&ToCountry='" + ToCountry + "'";
                        // urlString+= "&ToState='" + ToState + "'";
                        // urlString+= "&ToZip='" + ToZip + "'";
                        // urlString+= "&SmsStatus='" + SmsStatus + "'";
                        // urlString+= "&SMsMessageSid='" + SMsMessageSid + "'";
                        // urlString+= "&SmsSid='" + SmsSid + "'";
                        // urlString+= "&ApiVersion='" + ApiVersion + "'";
                        // urlString+= "&NewData='" + Body + "'";

                        // urlString+= "&Email='" + Email + "'";
                        // urlString+= "&Name='" + Name + "'";
                        // urlString+= "&Password='" + Password + "'";
                        // urlString+= "&SpecialInstructions='" + SpecialInstructions + "'";
                        // urlString+= "&OrderDetails='" + OrderDetails + "'";
                        // urlString+= "&DeliveryAddress='" + DeliveryAddress + "'";
                        // urlString+= "&Total='" + Total + "'";
                    
                        //vaout+=Uri.EscapeDataString(urlString);
                        //vaout+=urlString;
                        //vaout+="</Redirect>";
                                    vaout += "</Message>";
                                    vaout += "</Response>";
                                    Response.ContentType="text/xml";
                                    if(InDeliveryArea ){ 
                                        return vaout;
                                    }
                           }
                           return vaout;
                        
                            
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return httpRequestException.Message ;
                    }
                }








                List<DeliveryArea> Das = _context.DeliveryAreas.ToList();
                
                foreach (var area in Das){
                     
                }


            }
            Body=Body.Replace(" ","");
            if (Body.ToUpper().Contains("SENDME") || Body.ToUpper().Contains("SEND ME")){
                string[] orderItems = Body.Split(',');
                if (orderItems.Length>0){
                    if (orderItems[0].ToUpper().Contains("SENDME") || orderItems[0].ToUpper().Contains("SEND ME")){
                        var firstItem=-1;
                        int thisInventoryId=-1;
                        if (orderItems[0].Length>0){
                            if (orderItems[0].Substring( orderItems[0].ToUpper().LastIndexOf('E')+1).Length>0){
                               firstItem=Int32.Parse(orderItems[0].Substring( orderItems[0].ToUpper().LastIndexOf('E')+1));
                            }
                        }
                        orderItems[0]=firstItem.ToString();
                        if (orderItems.Length>1){
                            thisInventoryId = Int32.Parse(orderItems[1]);
                        }
                        
                        if (thisInventoryId>=0 || firstItem>=0){
                            //ApplicationUser ThisUser = _context.Users.Where(z=> From.Contains(z.PhoneNumber)).First();
                             ApplicationUser ThisUser = _context.Users.Where(z=> From.Contains(z.PhoneNumber)).FirstOrDefault();
                            if (ThisUser!=null){       
                            List<Inventory>  InventoryByAreaVar = (from d in _context.Inventorys
                                                        where orderItems.Contains(d.ID.ToString())
                                                        select  d
                                                        ).ToList();

                                Order newOrder = new Order();  
                                newOrder.AppUser=Guid.Parse(ThisUser.Id);
                                newOrder.GUID=Guid.NewGuid();	
                                newOrder.GeocodedAddress=ThisUser.DeliveryAddress;
                                newOrder.OrderDate=DateTime.Now;
                                newOrder.PaymentType=5;
                                newOrder.Status=-1;
                                newOrder.SpecialInstructions="";
                                newOrder.Total=0;
                                newOrder.DeliveryDate=DateTime.Now;
                                decimal taxes = Convert.ToDecimal(1.13);
                                decimal DeliveryCharge = Convert.ToDecimal(5);
                                string item_code ="";
                                foreach (var item in InventoryByAreaVar){
                                    //  orderItemStr+= "ID " + item.ToString() + (char)10+(char)13;
                                    string thisId=item.ID.ToString(); 
                                    foreach (var orderIDs in orderItems){
                                        if (orderIDs==thisId){
                                            string orderItemStr= "" + (char)10 + item.Label + "@ $" +  item.Price.ToString();
                                            newOrder.Details+=orderItemStr;
                                            newOrder.Total+=Convert.ToDecimal(item.Price);
                                        }
                                    }
                                }
                                var testvar = (
                                            from o in orderItems 
                                            group o by o into g
                                            orderby g.Count() descending
                                            select g.Count() +"x" +g.Key
                                        );
                                foreach (var codevar in testvar){
                                    item_code+=codevar+",";
                                }        
                                
                                newOrder.PhoneNumber=item_code;
                                newOrder.Total=(newOrder.Total + DeliveryCharge)*taxes;
                                _context.Add(newOrder);
                                await _context.SaveChangesAsync();
                                vaout = "";
                                vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                                vaout += "<Response>";
                                vaout += "<Message>";
                                vaout += "The following items are ready for deliverey"+(char)10 + (char)10 + newOrder.Details + (char)10 + (char)10;
                                vaout += "Going to:" +  newOrder.GeocodedAddress +(char)10 + (char)10 ;
                                vaout += "The total after tax and delivery charges is $" + newOrder.Total + (char)10 + (char)10 ; 
                                vaout += "Confirm by texting the following exactly as shown 'confirm " + newOrder.ID + "'";
                                vaout += "</Message>";
                                vaout += "</Response>";
                                Response.ContentType="text/xml";
                                return  vaout;
                            }
                        }
                    }    
                    //Something went wrong got sendme but didnt parse properly   
                    vaout = "";
                    vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                    vaout += "<Response>";
                    vaout += "<Message>";
                    vaout += "Sorry Something went wrong with your order please use the folowwing format:  'sendme,3,3,1' where you want to order 2 of item 3, ";
                    vaout += "and 1 of item 1. Delivered to your home address. " + (char)10 + (char)10 +" To see the menu text 'order'";
                    vaout += "</Message>";
                    vaout += "</Response>";
                    Response.ContentType="text/xml";
                     return  vaout;
                }
                 
                
                
            }else if (Body.ToUpper()!=("ORDER")){

                vaout = "";
                vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                vaout += "<Response>";
                vaout += "<Message>";
                vaout += "UberDuber Delivery." + (char)10 + (char)13 + "If you would like a delivery reply 'order'" + (char)10;
                vaout +=  "Use 416-802-8129 to voice order Thanks";
                vaout += "</Message>";
                vaout += "</Response>";
                Response.ContentType="text/xml";
                return  vaout;
            }else{
               ApplicationUser ThisUser = _context.Users.Where(z=> From.Contains(z.PhoneNumber)).FirstOrDefault();
               
               
               if (ThisUser==null){
                        vaout = "";
                        vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                        vaout+="<Response>";
                        vaout+="<Message>";
                        vaout+="Hi " + From  + (char)10 + "We could not find your sms number in our database.  If you want to register for a delivery we have a quick few questions to get your order on its way."+ (char)10 +(char)10+ "  First lets check your address. " + (char)10 + "Please text your delivery address like this 'Address:123 yonge st. Toronto, Ontario, M5W5H6.";
                        vaout+="</Message>";
                        vaout+="</Response>";
                        Response.ContentType="text/xml";
                        return vaout;
                }else{
                        int userDA=ThisUser.DeliveryAreaId;
                        string menuStr = ""  ;
                        List<InventoryByArea>  InventoryByAreaVar = (from d in _context.Inventorys
                                                join ig in _context.InventoryGroups on  d.ID equals    ig.InventoryId      // first join
                                                join da in _context.DeliveryAreas on ig.DeliveryAreaId equals da.ID     // second join
                                                where da.ID == userDA
                                                where d.Quantity > 0
                                                orderby da.Name
                                                select  new InventoryByArea
                                                {
                                                Label=d.Label, Name=da.Name,ID=da.ID,Quantity= d.Quantity,Price=d.Price,ImageFilePath=d.ImageFilePath, InventoryId=d.ID
                                                }).ToList();
                        foreach (var item in InventoryByAreaVar){
                                menuStr+= "ID " + item.InventoryId + " = " +  item.Label + " @ $" + item.Price + (char)10; 
                        }
                        string DeliveryAddress =ThisUser.DeliveryAddress;
                        string Name=ThisUser.FirstName;
                        string Email = ThisUser.Email;
                        string Password="";
                        string SpecialInstructions="";
                        string OrderDetails = menuStr;
                        string Total="";

                        vaout = "";
                        vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                        vaout+="<Response>";
                        vaout+="<Message>";
                        vaout+="Hi " + Name + (char) 10 +  DeliveryAddress  + (char) 10;
                        //vaout+="To select your items enter the format quantity1xID1,quantity2xID2... eg 3x1,1x6  means 3 x item1 and 1 x item6 from the fllowing menu.  We will confirm your order";
                        vaout+="The following items are available in your area right now"  + (char)10 +  (char)13 ;
                        vaout+=menuStr;
                        vaout+=  (char)10 +"To order items text 'sendme,' followed by the item Id's you want seperated by commas " + (char)10 + "Example text 'send me3,3,1'  means 2 x item3 1 x item1";

                        vaout+="</Message>";
                        //vaout+="<Redirect>";
                        // urlString+= "SmsOut?From='" + From + "'";
                        // urlString+= "&To='" + From + "'";
                        // urlString+= "&Body='" + Body + "'";
                        // urlString+= "&AccountSid='" + AccountSid + "'";
                        // urlString+= "&FromCity='" + FromCity + "'";
                        // urlString+= "&FromCountry='" + FromCountry + "'";
                        // urlString+= "&FromState='" + FromState + "'";
                        // urlString+= "&FromZip='" + FromZip + "'";
                        // urlString+= "&ToCity='" + ToCity + "'";
                        // urlString+= "&ToCountry='" + ToCountry + "'";
                        // urlString+= "&ToState='" + ToState + "'";
                        // urlString+= "&ToZip='" + ToZip + "'";
                        // urlString+= "&SmsStatus='" + SmsStatus + "'";
                        // urlString+= "&SMsMessageSid='" + SMsMessageSid + "'";
                        // urlString+= "&SmsSid='" + SmsSid + "'";
                        // urlString+= "&ApiVersion='" + ApiVersion + "'";
                        // urlString+= "&NewData='" + Body + "'";

                        // urlString+= "&Email='" + Email + "'";
                        // urlString+= "&Name='" + Name + "'";
                        // urlString+= "&Password='" + Password + "'";
                        // urlString+= "&SpecialInstructions='" + SpecialInstructions + "'";
                        // urlString+= "&OrderDetails='" + OrderDetails + "'";
                        // urlString+= "&DeliveryAddress='" + DeliveryAddress + "'";
                        // urlString+= "&Total='" + Total + "'";
                    
                        //vaout+=Uri.EscapeDataString(urlString);
                        //vaout+=urlString;
                        //vaout+="</Redirect>";
                        //vaout+="</Message>";
                        vaout+="</Response>";
                        Response.ContentType="text/xml";
                         return  vaout;
                }
            } 
            return  vaout;
        }
         public string SMSOut(string From ,string Body ,string AccountSid,string FromCity,
                                                string FromCountry,string FromState,string FromZip,string ToCity,
                                                string ToCountry,string ToState,string ToZip,string SmsStatus,
                                                string To,string SMsMessageSid,string SmsSid,string ApiVersion,
                                                string NewData="",string Email="",string Name="",string Password="",
                                                string SpecialInstructions="",string OrderDetails="",string DeliveryAddress="",string Total="" 
                                                )
        {       
                string vaout="";
                string hostStr="Https://" + Request.Host;
                
                vaout+= hostStr + "/messages/SmsOut?From='" + From + "'";
                vaout+= "&To='" + To + "'";
                vaout+= "&Body='" + Body + "'";
                vaout+= "&AccountSid='" + AccountSid + "'";
                vaout+= "&FromCity='" + FromCity + "'";
                vaout+= "&FromCountry='" + FromCountry + "'";
                vaout+= "&FromState='" + FromState + "'";
                vaout+= "&FromZip='" + FromZip + "'";
                vaout+= "&ToCity='" + ToCity + "'";
                vaout+= "&ToCountry='" + ToCountry + "'";
                vaout+= "&ToState='" + ToState + "'";
                vaout+= "&ToZip='" + ToZip + "'";
                vaout+= "&SmsStatus='" + SmsStatus + "'";
                vaout+= "&SMsMessageSid='" + SMsMessageSid + "'";
                vaout+= "&SmsSid='" + SmsSid + "'";
                vaout+= "&ApiVersion='" + ApiVersion + "'";
                vaout+= "&NewData='" + Body + "'";

                vaout+= "&Email='" + Email + "'";
                vaout+= "&Name='" + Name + "'";
                vaout+= "&Password='" + Password + "'";
                vaout+= "&SpecialInstructions='" + SpecialInstructions + "'";
                vaout+= "&OrderDetails='" + OrderDetails + "'";
                vaout+= "&DeliveryAddress='" + DeliveryAddress + "'";
                vaout+= "&Total='" + Total + "'";
               
             var UrlStr = vaout ;
            vaout = "Phone Number: " + From + (char) 10 + (char) + 13;
            vaout += "City: " + FromCity + (char) 10 + (char) + 13;
            vaout += "Province: " + FromState + (char) 10 + (char) + 13;
            vaout += "Country: "+ FromCountry + (char) 10 + (char) + 13;
            vaout += "Postal Code:"+ FromZip + (char) 10 + (char) + 13;
            vaout += "Body: " + Body + (char) 10 + (char) + 13;
            vaout += "Status: " + SmsStatus + (char) 10 + (char) + 13;
            vaout += UrlStr;
            Response.ContentType="text/plain";
            return vaout;
        }
         public async Task<string> SMS_Voice_Reply(string From ,string Body ,string AccountSid,string FromCity,
                                                string FromCountry,string FromState,string FromZip,string ToCity,
                                                string ToCountry,string ToState,string ToZip,string SmsStatus,
                                                string To,string SMsMessageSid,string SmsSid,string ApiVersion,
                                                string NewData="",string Email="",string Name="",string Password="",
                                                string SpecialInstructions="",string OrderDetails="",string DeliveryAddress="",string Total="" 
                                                )
        {
            string vaout = "";
            vaout += "<?xml version='1.0' encoding='UTF-8'?>";
            vaout+="<Response>";
            vaout+="<Sms>";
            vaout+="This sms number is for SMS ordering and outgoing notifications only.  Please text the 'word' order to this number " + To + " for instructionsor.  Or call 416-802-8129 to get in touch with UberDuber Order Desk.  Thanks";
            vaout+="</Sms>";
            vaout+="</Response>";

            return vaout;
        }


        private class Points{
            public  Double latitude {get;set;} 
            public  Double longitude {get;set;}
        }
        private bool pointInPolygon(List<Points> points,double lat ,double lon) {
            var i=0;
            var j=points.Count()-1;
            var inPoly=false;

            for (i=0; i<points.Count(); i++) 
            {
                if (points[i].longitude<lon && points[j].longitude>=lon 
                || points[j].longitude<lon && points[i].longitude>=lon) 
                {
                if (points[i].latitude+(lon-points[i].longitude)/ 
                    (points[j].longitude-points[i].longitude)*(points[j].latitude 
                    -points[i].latitude)<lat) 
                {
                    inPoly=!inPoly; 
                }
                }
                j=i; 
            }
            return inPoly; 
        }
    }
}

