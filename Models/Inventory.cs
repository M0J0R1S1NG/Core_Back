using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Inventory
    {
        public int ID { get; set; }
 
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        public string Label { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        public string Description { get; set; }

        public int Status { get; set; }
        public bool OnHand { get; set; }
        
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMM dd, yyyy}")]
        public DateTime BestBefore { get; set; }
        
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMM dd, yyyy}")]
        public DateTime OrderDate { get; set; }
         
        public double Quantity { get; set; }

        [DisplayFormat(DataFormatString="{0:C}")]
        public double Price { get; set; }

        [DisplayFormat(DataFormatString="{0:C}")]
        public double Cost { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        public string Supplier { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        public string Notes { get; set; }
        public Byte[] Photo { get; set; }
        
        [DisplayFormat(DataFormatString="{0}%")]
        public double THCContent { get; set; }
        
        [DisplayFormat(DataFormatString="{0}%")]
        public double CBDContent { get; set; }
        public int Likes { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        public string ImageFilePath {get;set;}
        [DisplayFormat(DataFormatString="{0:C}")]
        public int PricePerGram {get;set;}
        [DisplayFormat(DataFormatString="{0:C}")]
        public int PricePerQuarter {get;set;}
        [DisplayFormat(DataFormatString="{0:C}")]
        public int PricePerhalf {get;set;}
        [DisplayFormat(DataFormatString="{0:C}")]
        public int PricePerOz {get;set;}
        [DisplayFormat(DataFormatString="{0:C}")]
        public int CostPerGram {get;set;}
        public int Discount {get;set;}
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        public string UPC {get;set;}
        public string Qualities {get;set;}
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        
        public string catagory {get;set;} 
        public int PartnerId {get;set;}
        
    }
}
