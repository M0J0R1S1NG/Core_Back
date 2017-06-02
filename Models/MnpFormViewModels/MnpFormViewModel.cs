using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.ManageViewModels;
using Core.Models.AccountViewModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Core.Models.MnpFormViewModels;

namespace Core.Models.MnpFormViewModels
{
    public class MnpFormViewModel
    {       
              
        [Display(Name = "Id")]
        public int Id {get;}

        [Display(Name = "Application User")]
        public  Guid UserId { get; set; }
        
        [Display(Name = "Medical Conditions")]
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true)]
        public  bool MedConditionsExisting {get;set;}
        
        [Display(Name = "List of Conditions")]
        public string MedConditions {get;set;}
        
        [Display(Name = "Condition Treatments")]
        public string MedConditionsTreatment {get; set;}
        
        [Display(Name = "Exiting Marijuana Treatment")]
        public bool MedCannabisUser {get;set;}
        
        [Display(Name = "Length of use")]        
        public int YearsUsing {get;set;}
        
        [Display(Name = "Relieves Conditions?")]
        public bool RelievesCondition {get;set;}
        [Display(Name = "Has Alergies")]        
        public bool HasAlergies {get; set;}
        
        [Display(Name = "Alergies")]        
        public string Alergies {get; set;}
        
        [Display(Name = "Singature Provided")]
        public bool Signature {get;set;} 

        //[Required(ErrorMessage = "Please sign the image box with your touch screen or mouse")]
        [DataType(DataType.Upload)]
        [Display(Name = "Signature block")]
        [FileExtensions(Extensions = "jpg,gif,png")]
        public IFormFile SignatureFile { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:dddd, MMMM dd, yyyy}")]
        public DateTime  Date {get;set;}

        [DisplayFormat]
        [Display(Name = "Photo ID Provided")]
        public bool PhotoId {get; set;}

        //[Required(ErrorMessage = "Please Upload a Valid Image File. Only jpg format allowed")]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Your Photo I.D Image")]
        [FileExtensions(Extensions = "jpg,gif,png")]
        public IFormFile Image { get; set; }

        [Display(Name = "Image File Name")]
        public string ImageName { get; set; } 
        
        [Display(Name = "Legal First Name(s)")]
        public virtual string LegalFirstName { get; set; } 
        
        [Display(Name = "Legal Middle Name(s)")]
        public virtual string LegalMiddleName { get; set; } 
        
        
        [Display(Name = "Legal Last Name(s)")]
        public virtual string LegalLastName { get; set; } 
        public virtual string Doctor { get; set; } 
        public virtual string DoctorCity { get; set; } 
        public virtual string Dispencery { get; set; } 
        public virtual bool WantRx { get; set; } 
        
    }
}