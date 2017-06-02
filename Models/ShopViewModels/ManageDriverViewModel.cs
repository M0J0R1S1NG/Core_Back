
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.ManageViewModels;
using Core.Models.AccountViewModels;

using System.ComponentModel.DataAnnotations;



namespace Core.Models
{
    public class ManageDriverViewModel
    {
         
        [Required]
        [EmailAddress]
        public string Email { get; set; }

  
        public int ID { get; set; }
        public Guid GUID { get; set; }
        public Guid UserGuid { get; set; }
        public IList<ApplicationUser> AppUsers { get; set; }
        public  IList<Address> GeocodedAddresses { get; set; }
        public string Address1 { get; set; }
        public string PostalCode { get; set; }
        public string Details { get; set; }
        public string SpecialInstructions { get; set; }
        public int Status { get; set; }
        public string PhoneNumber { get; set; }
        public IList<Vehicle> Vehicles { get; set; }

    }
}

