using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class InventoryGroup{
            public int Id {get;set;}

            public int InventoryId  {get;set;}
            public int DeliveryAreaId {get;set;}



    }
}