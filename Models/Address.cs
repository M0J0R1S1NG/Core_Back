using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Core.Models
{
    public class Address
    {
        [Key]
        public int ID { get; set; }
        public string Name {get;set;}
        public Guid CreatedUser { get; set; }
        public Boolean Geocoded { get; set; }
        public string FullAddress { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string  PhoneNumber { get; set; }
        public string SpecialInstructions { get; set; }
        public int Status { get; set; }
       
    }
}
