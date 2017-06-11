using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
namespace Core.Models
{
    public class DeliveryAreaDriver
    {
        [Key]
        public int Id {get;set;}
        public Driver DriverId {get;set;}
        public DeliveryArea DeliverAreaId {get;set;}
    
    }
}