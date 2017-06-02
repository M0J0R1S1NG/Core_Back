using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Core.Models.ManageViewModels
{
    public class UpdateUserViewModel
    {
        
        public string Id;

        [Required]
        [Display(Name = "Delivery Address")]
        public String DeliveryAddress;

        [Required]
        [Display(Name = "Street Name")]
        public String StreetName;
        
        [Required]
        [Display(Name = "Street Number")]
        public String StreetNumber;
                
        [Required]
        [Display(Name = "Postal Code")]
        public String PostalCode;
        
        [Required]
        [Display(Name = "City")]
        public String City;

        [Required]
        [Display(Name = "Province")]
        public String Province;

        [Required]
        [Display(Name = "Country")]
        public String Country;

        [Required]
        [Display(Name = "Birth Date")]
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMMdd, yyyy}")]
        [DataTypeAttribute(DataType.Date)]
        public DateTime DoB;

    }
}
