using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.ManageViewModels;
using Core.Models.AccountViewModels;
using System.ComponentModel.DataAnnotations;



namespace Core.Models
{
    public class ManageOrderViewModel
    {
         
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public IList<ApplicationUser> AppUsers { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public  IList<Address> GeocodedAddresses { get; set; }   
        public string PhoneNumber { get; set; }
        public decimal Total { get; set; }
        public decimal Weight { get; set; }
        public int PaymentType { get; set; }
        public string Details { get; set; }
        public string SpecialInstructions { get; set; }
        public int Status { get; set; }
         public IList<Driver> Drivers { get; set; }


    }
}


