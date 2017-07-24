
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class UserCredits
    {
       public UserCredits()
        {
                
        }
        public int Id {get;set;}
        public int  OrderId {get;set;}
        public string TransactionId {get;set;}
        public Guid UserGuid {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}

        public double TotalCredit {get;set;}
        public double StoreCredit {get;set;}
        public double ReferralCredit {get;set;}
        
    }
}