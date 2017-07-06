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
using Twilio;
using Twilio.TwiML;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


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

        public MessagesController(ApplicationDbContext context,
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
        // GET: Partners

       public async Task<string> SMS_Text_Reply(string From ,string Body ,string AccountSid,string FromCity,
                                                string FromCountry,string FromState,string FromZip,string ToCity,
                                                string ToCountry,string ToState,string ToZip,string SmsStatus,
                                                string To,string SMsMessageSid,string SmsSid,string ApiVersion)
        {
            var vaout="";
            var urlString = "";
            var bodyStr= Body.ToUpper();
            if (bodyStr.StartsWith("SENDME")){
                var nothing=1;
                var items=bodyStr.Split(',');
                for (var i=0;i<=items.length();i++)
                {
                        
                }
            }else if (bodyStr.StartsWith("ORDER")){

                vaout = "";
                vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                vaout+="<Response>";
                vaout+="<Message>";
                vaout+="UberDuber Delivery." + (char) 10 + (char) + 13 + "If you would like to order- reply 'order' or text 'order' to 647-799-2699." + (char) 10 + (char) + 13 + "Use 416-802-8129 to get an UberDuber representative.  Thanks";
                vaout+="</Message>";
                vaout+="</Response>";
                Response.ContentType="text/xml";
            }else{
               ApplicationUser ThisUser = _context.Users.Where(z=> From.Contains(z.PhoneNumber)).First();
               if (ThisUser==null)
                {
                        vaout = "";
                        vaout += "<?xml version='1.0' encoding='UTF-8'?>";
                        vaout+="<Response>";
                        vaout+="<Message>";
                        vaout+="Hi " + From  + " We just have a few questions to get your order together.  First what's your name?  Please type all replys on a single line";
                        vaout+="</Message>";
                        vaout+="</Response>";
                        Response.ContentType="text/xml";
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
                        vaout+=  (char)10 +"To order items text 'sendme,' followed by the item Id's you want seperated by commas " + (char)10 + "Example text 'sendme,3,3,1'  means 2 x item3 1 x item1";

                        vaout+="</Message>";
                        vaout+="<Redirect>";
                        urlString+= "SmsOut?From='" + From + "'";
                        urlString+= "&To='" + From + "'";
                        urlString+= "&Body='" + Body + "'";
                        urlString+= "&AccountSid='" + AccountSid + "'";
                        urlString+= "&FromCity='" + FromCity + "'";
                        urlString+= "&FromCountry='" + FromCountry + "'";
                        urlString+= "&FromState='" + FromState + "'";
                        urlString+= "&FromZip='" + FromZip + "'";
                        urlString+= "&ToCity='" + ToCity + "'";
                        urlString+= "&ToCountry='" + ToCountry + "'";
                        urlString+= "&ToState='" + ToState + "'";
                        urlString+= "&ToZip='" + ToZip + "'";
                        urlString+= "&SmsStatus='" + SmsStatus + "'";
                        urlString+= "&SMsMessageSid='" + SMsMessageSid + "'";
                        urlString+= "&SmsSid='" + SmsSid + "'";
                        urlString+= "&ApiVersion='" + ApiVersion + "'";
                        urlString+= "&NewData='" + Body + "'";

                        urlString+= "&Email='" + Email + "'";
                        urlString+= "&Name='" + Name + "'";
                        urlString+= "&Password='" + Password + "'";
                        urlString+= "&SpecialInstructions='" + SpecialInstructions + "'";
                        urlString+= "&OrderDetails='" + OrderDetails + "'";
                        urlString+= "&DeliveryAddress='" + DeliveryAddress + "'";
                        urlString+= "&Total='" + Total + "'";
                    
                        vaout+=Uri.EscapeDataString(urlString);
                        //vaout+=urlString;
                        vaout+="</Redirect>";
                        //vaout+="</Message>";
                        vaout+="</Response>";
                        Response.ContentType="text/xml";
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
         public async Task<string> SMS_Voice_Reply()
        {
            string vaout = "";
            vaout += "<?xml version='1.0' encoding='UTF-8'?>";
            vaout+="<Response>";
            vaout+="<Sms>";
            vaout+="This sms number if for outgoing messages only.  You can call or text 416-802-8129 to get in touch with UberDuber Delivery Services.  Thanks";
            vaout+="</Sms>";
            vaout+="</Response>";

            return vaout;
        }

    }
}
