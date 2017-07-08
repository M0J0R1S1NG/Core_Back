using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;


namespace Core.Models
{
    public class AppSettings
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Setting or Variable Name")]
        public string VariableName { get; set; }
        [Display(Name = "Variable Declared Type")]
        public string VariableType { get; set; }
        [Display(Name = "Value (ASCII String)")]
        public string VariableValue { get; set; }
        public string Status { get; set; }
                
    }
}