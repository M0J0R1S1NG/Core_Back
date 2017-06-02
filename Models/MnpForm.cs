using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.ManageViewModels;
using Core.Models.AccountViewModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.Models
{
    public class MnpForm
    {   
       [Key] 
        public int Id {get;set;}
       
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMM dd, yyyy}")]
        public DateTime  Date {get;set;}

        public Guid UserId { get; set; }

        [Display(Name = "Are you currently taking medications for your symptoms/conditions")]
        public bool MedConditionsExisting {get;set;}
        
        [Display(Name = "Please describe your condition and symptoms that requires treatment:")]
        public string MedConditions {get;set;}
        
        [Display(Name = "Condition Treatments")]
        public string MedConditionsTreatment {get; set;}
        
        [Display(Name = "Have you used Marijuana")]
        public bool MedCannabisUser {get;set;}
        
        [Display(Name = "How long have you used Marijuana")]  
        public int YearsUsing {get;set;}
        
        [Display(Name = "Does it relieves your conditions?")]
        public bool RelievesCondition {get;set;}
        
        [Display(Name = "Do you have any alergies")] 
        public bool HasAlergies {get; set;}
        
        [Display(Name = "Please list alergies")]  
        public string Alergies {get; set;}

        [Display(Name = "Singature Provided")]
        public bool Signature {get;set;} 

      
        [DataType(DataType.Upload)]
        [Display(Name = "Please sign in the box")]
        public byte[] SignatureFile { get; set; }

        [Display(Name = "Photo ID Provided")]
        public bool PhotoId {get; set;}

        
        [DataType(DataType.Upload)]
        [Display(Name = "Please upload your photo Id.")]
        public byte[] Image { get; set; }

        [Display(Name = "Image File Name")]
        public string ImageName { get; set; }
        [Display(Name = "Legal First Name(s)")]
        public virtual string LegalFirstName { get; set; }
        [Display(Name = "Legal Last Name(s)")]
        public virtual string LegalLastName { get; set; } 
        [Display(Name = "Doctors Name(s)")]
        public virtual string Doctor { get; set; } 
        [Display(Name = "Your Dr.s City")]
        public virtual string DoctorCity { get; set; } 
        [Display(Name = "Your dispencaries")]
        public virtual string Dispencery { get; set; } 
        
        [Display(Name = "Please check here if you would like us to submit your information to a medical practitioner and to Health Canada for an MMAR")]
        public virtual bool WantRx { get; set; } 
    }
}