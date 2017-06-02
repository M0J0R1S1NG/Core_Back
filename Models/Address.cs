using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Address
    {

        public int ID { get; set; }
        public Guid GUID { get; set; }
        public Guid CreatedUser { get; set; }
        public Boolean Geocoded { get; set; }
        public string FullAddress { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public decimal PhoneNumber { get; set; }
        public string SpecialInstructions { get; set; }
        public int Status { get; set; }
       
    }
}
