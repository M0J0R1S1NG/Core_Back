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

       public async Task<string> SMS_Text_Reply(string From ,string Body ,string AccountSid,string FromCity,string FromCountry, 
                                                string FromState,string FromZip,string ToCity,string ToCountry, 
                                                string ToState,string ToZip,string SmsStatus,string To,string SMsMessageSid, 
                                                string SmsSid,string ApiVersion,string NewData="" )
        {
            // string vaout = "";
            // vaout += "<?xml version='1.0' encoding='UTF-8'?>";
            // vaout+="<Response>";
            // vaout+="<Sms>";
            // vaout+="This sms number if for outgoing messages only.  You can call or text 416-802-8129 to get in touch with UberDuber Delivery Services.  Thanks";
            // vaout+="</Sms>";
            // vaout+="</Response>";

            //var Msgresponse = new  MessagingResponse();
        
            //Msgresponse.Message("TestLink", action: "/Messages/Sms_Text_Reply?NewData=11119999", method: "GET");
            
            //Msgresponse.Redirect("GET", "/Messages/Sms_Text_Reply?NewData=11119999");
            //System.Console.WriteLine(Msgresponse.ToString());
            // var User =  _context.Users.Where(x=> x.PhoneNumber==From).SingleOrDefault();
           
            // if (User.PhoneNumber.Length<1){
            //    //New user
            //         User.PhoneNumber=From;
            //         User.City=FromCity;
            //         User.Country=FromCountry;
            //         User.PostalCode=FromZip;
            //         User.Province=FromState;

            // }else{
            //    //Existing User
            //    //var AppUser=_signInManager.SignInAsync(User,true);

            // }
            
            
            var User =  _context.Users.Where(x=> x.PhoneNumber==From).SingleOrDefault();


            string message_body = Body;
            string from_number = From;
            string vaout = "";
            vaout += "Phone Number: " + From + (char) 10 + (char) + 13;
            vaout += "City: " + FromCity + (char) 10 + (char) + 13;
            vaout += "Province: " + FromState + (char) 10 + (char) + 13;
            vaout += "Country: "+ FromCountry + (char) 10 + (char) + 13;
            vaout += "Postal Code:"+ FromZip + (char) 10 + (char) + 13;
            vaout += "Body: " + Body + (char) 10 + (char) + 13;
            vaout += "Status: " + SmsStatus + (char) 10 + (char) + 13;
            Response.ContentType="text/plain"; 
             
             
             
             
             
             
             vaout = "";
            vaout += "<?xml version='1.0' encoding='UTF-8'?>";
            vaout+="<Response>";
            // vaout+="<Sms>";
            // vaout+="This sms number is for outgoing messages only.  You can call or text 6477992699 to get in touch with UberDuber Delivery Services.  Thanks";
            // vaout+="</Sms>"; 
            vaout+="<Message>Hello World!</Message>";
            vaout+="<Redirect>https://www.uberduber.net/Messages/SmsOut?From=me$body=thisbody</Redirect>";
            
            vaout+="</Response>";
            Response.ContentType="text/xml";
            



            vaout = "http://www.uberduber.net/Messages/SMSOut?NewData=Helloworld";
            Response.ContentType="text/plain";
            //Response.Body="<a href=/Messages/SMSOut?From=6475284350&body=Helloworld>TestLink</a>";





            vaout = "";
            vaout += "<?xml version='1.0' encoding='UTF-8'?>";
            vaout+="<Response>";
            vaout+="<Message>";
            vaout+="This sms number is for UberDuber delivery ordering if you would like to proceed text the word 'order' back to this number 647-799-2699. Otherwise call or text 416-802-8129 to get in touch with an UberDuber live representative.  Thanks";
            vaout+="</Message>";
            vaout+="<Redirect>SmsOut?NewData=TestingData</Redirect>";
            vaout+="</Response>";
            Response.ContentType="text/xml";
          
            return  vaout;
        }
         
         
         
         
         public string SMSOut(string From ,string Body ,string AccountSid,string FromCity,string FromCountry, 
                                                string FromState,string FromZip,string ToCity,string ToCountry, 
                                                string ToState,string ToZip,string SmsStatus,string To,string SMsMessageSid, 
                                                string SmsSid,string ApiVersion,string NewData="" )
        {
            string message_body = Body;
            string from_number = From;
            string vaout = "/SMSOut?NewData=" + NewData + (char) 10 + (char) + 13;
            
            
            vaout += "Phone Number: " + From + (char) 10 + (char) + 13;
            vaout += "City: " + FromCity + (char) 10 + (char) + 13;
            vaout += "Province: " + FromState + (char) 10 + (char) + 13;
            vaout += "Country: "+ FromCountry + (char) 10 + (char) + 13;
            vaout += "Postal Code:"+ FromZip + (char) 10 + (char) + 13;
            vaout += "Body: " + Body + (char) 10 + (char) + 13;
            vaout += "Status: " + SmsStatus + (char) 10 + (char) + 13;
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
