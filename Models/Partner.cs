using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.ManageViewModels;
using Core.Models.AccountViewModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Partner
    {
        
        public int Id { get; set; }
        public Guid GUID { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string SpecialInstructions { get; set; }
        [Display(Name = "Status set to 1 if partner has drivers set to 2 if Single Partner Dispatch set -1 to disable")]        
        public int Status { get; set; }
        public int DeliveryArea {get;set;}
        public string SMSNumber {get;set;}
        public string ShippingAddress {get;set;}
        public string Name {get; set;}
        public string Company {get;set;}
        public string TaxId {get;set;}
        public string EmailAddress {get;set;}

        
    }
}
