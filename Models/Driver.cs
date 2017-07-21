using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Core.Data;

namespace Core.Models
{
    public class Driver
    {
        [Key]

        public int ID { get; set; }

        public Guid UserGuid { get; set; }

        public string GeocodedAddress { get; set; }

        public string Address1 { get; set; }

        public string PostalCode { get; set; }

        public string Details { get; set; }

        public string SpecialInstructions { get; set; }

        public int Status { get; set; }

        public string PhoneNumber { get; set; }

        public int VehicleId { get; set; }

        public string LicenseProvince {get ;set;}
        public string LicenseNumber {get;set;}
        public int PartnerId {get;set;}

        public string EmailAddress {get;set;}

        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:hh:mm}")]
        public DateTime StartTime {get;set;}
        
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:hh:mm}")]
        public DateTime EndTime {get;set;}
        public double Hours {get;set;}
        
    }
}