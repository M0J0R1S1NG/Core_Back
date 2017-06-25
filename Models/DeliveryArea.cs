using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
namespace Core.Models
{
    public class DeliveryArea
    {

        public int ID { get; set; }
       
        public Guid CreatedBy { get; set; }

        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMM dd, yyyy}")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Partner")]
        public int PartnerId { get; set; }

        [Display(Name = "Partner User")]
        public Guid Partner { get; set; }

        [Display(Name = "Delivery area polygon points")]
        public string Points {get;set;}

        public string Name {get;set;}

        public string Description {get;set;}

        [Display(Name = "Opens at")]
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0: hh:mm}")]
        public DateTime OpenTime {get;set;}
        [Display(Name = "Closes at")]
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0: hh:mm}")]
        public DateTime ClosedTime {get;set;}
        [Display(Name = "Open/closed?")]
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:Open/Closed}")]
        public bool Open {get;set;}
        
        IList<Driver> Drivers {get;set;}
        public int Status { get; set; }
      
    }
}
