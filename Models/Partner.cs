using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public int Status { get; set; }
        public int DeliveryArea {get;set;}
        public string SMSNumber {get;set;}
        public string ShippingAddress {get;set;}
        public string Name {get; set;}
        public string Company {get;set;}
        public string TaxId {get;set;}
        
    }
}
