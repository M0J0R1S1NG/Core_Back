using System;
using System.ComponentModel.DataAnnotations;
namespace Core.Models
{
     public class Franchise 
    {

        public Franchise()
        {
            ContactDate=DateTime.Now;
            StartDate=DateTime.Now;
        }

        [Key]
        public int Id {get;set;}
        public Guid AppUser {get;set;}
        [Display(Name="City or Territory Desired")]        
        public string City {get;set;}

        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMM dd, yyyy}")]
        [Display(Name="Seniority Date")]
        public DateTime ContactDate {get;set;}
        [Display(Name="Do you have a vehicle (not required)")]
        public bool HasVehicle  {get;set;}
        [Display(Name="Vehicle Model")]
        public string VehicleType {get;set;}

        [Display(Name="Vehicle Year")]
        public int VehicleYear {get;set;}
        [Display(Name="Can we do a criminal background check?")]
        public bool Consent {get;set;}
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMM dd, yyyy}")]
        [Display(Name="When can you start")]
        public DateTime StartDate {get;set;}
    }
}