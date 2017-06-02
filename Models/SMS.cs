using System;
using System.ComponentModel.DataAnnotations;
namespace Core.Models
{
     public class SMS 
    {

        [Key]
        public Guid Id {get;set;}
       
        [Display(Name="User Guid")]        
        public Guid AppUser {get;set;}
       
        [Display(Name="Sent To Number")]        
        public String SentTo {get;set;}

        [Display(Name="Sent From Number")]        
        public String SentFrom {get;set;}
       
        [Display(Name="Date Sent")]        
        public String DateSent {get;set;}
 
        [Display(Name="Date Recieved")]        
        public String DateRecieved {get;set;}

 
    }
}