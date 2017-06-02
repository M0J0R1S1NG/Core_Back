using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


using System.ComponentModel.DataAnnotations;
using Core.Data;

namespace Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public String StreetName {get;set;}
        public String PostalCode {get;set;}
        public String StreetNumber {get;set;}
        public String City {get;set;}
        public String DeliveryAddress {get; set;}
        public String Province {get;set;}
        public String Country {get;set;}
        
        [Display(Name = "Birth Date")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMMdd, yyyy}")]
        public DateTime DoB {get;set;}
        public int status {get;set;}
        public bool Mnp {get;set;}
        public Guid MnpGuid {get;set;}
         

    }
    
}
