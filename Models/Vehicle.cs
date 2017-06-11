using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Core.Models
{
    public class Vehicle
    {

        public int ID { get; set; }
        [Display(Name = "User Account")]
        public Guid UserGuid { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Details { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string LicensePlate {get;set;}
        public string InsurancePolicy {get;set;}
    }
}
