using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string accountSid { get; set; }
        public string authToken { get; set; }
        public string gmailUser { get; set; }
        public string gmailPassword { get; set; }
    }
}
