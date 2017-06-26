using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class InventoryByArea
    {
       public InventoryByArea()
        {
                
        }
        
         public string Label {get;set;}
          public string Name {get;set;}
          public int ID {get;set;}
          public double Quantity {get;set;}
          public double Price {get;set;}
          public string ImageFilePath {get;set;}

    }
}