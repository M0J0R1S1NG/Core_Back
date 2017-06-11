using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Core.Data;

namespace Core.Models
{
        public class DeliveryAreaOrder{
            public Order order {get;set;}
            public DeliveryArea deliveryarea {get; set;}            
            public  Partner partner {get;set;}
            public ApplicationUser applicationuser {get;set;} 
            public Driver driver {get;set;}
            public Vehicle vehicle {get;set;}

        }

}