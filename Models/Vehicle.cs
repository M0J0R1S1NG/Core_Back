using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Vehicle
    {

        public int ID { get; set; }
        public Guid GUID { get; set; }
        public Guid UserGuid { get; set; }
        public string GeocodedAddress { get; set; }
        public string Address1 { get; set; }
        public string PostalCode { get; set; }
        public string Details { get; set; }
        public string SpecialInstructions { get; set; }
        public int Status { get; set; }
        public string PhoneNumber { get; set; }
        public Guid VehicleGuid { get; set; }
        
        

    }
}
