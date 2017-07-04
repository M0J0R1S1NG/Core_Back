using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;

namespace Core.Controllers
{
    
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Partners
        [HttpGet]
       public async Task<string> SMS_Text_Reply(string From ,string Body)
        {

            string message_body = Body;
            string from_number = From;
            string vaout = "";
            vaout += "<?xml version='1.0' encoding='UTF-8'?>";
            vaout += "<Response>";
            vaout += "<Sms>";
            vaout += from_number + "This sms number if for outgoing messages only.  You can call or text 416-802-8129 to get in touch with UberDuber Delivery Services.  Thanks";
            vaout += Body + "</Sms>";
            vaout += "<Media>https://UberDuber.com/images/uber_duber_square.png</Media>";
            vaout += "</Response>";

            return vaout;
        }
        [HttpGet]
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
