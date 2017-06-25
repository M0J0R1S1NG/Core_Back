using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


using System.ComponentModel.DataAnnotations;
using Core.Data;

namespace Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string  UnitNumber {get;set;}
        public String StreetName {get;set;}
        public String PostalCode {get;set;}
        [Display(Name = "Unit/Apt #")]
        public String StreetNumber {get;set;}
        public String City {get;set;}
        public String DeliveryAddress {get; set;}
        public int DeliveryAreaId {get;set;}
        public String Province {get;set;}
        public String Country {get;set;}
        
        [Display(Name = "Birth Date")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMMdd, yyyy}")]
        [DataType(DataType.DateTime)] 
        public DateTime DoB {get;set;}
        public int status {get;set;}
        public bool Mnp {get;set;}
        public Guid MnpGuid {get;set;}
         
         
       [Display(Name = "First and Middle Names")]
       public string FirstName {get;set;}
       [Display(Name = "Last Name")]
       public string LastName {get;set;}
       public string Language {get;set;}
        
      
    }
    
}
