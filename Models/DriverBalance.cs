using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Core.Data;

namespace Core.Models
{
    public class DriverBalance
    {
     
        [Key]
        public int ID { get; set; }

        public Guid merchant_transaction_id { get; set; }

        public int DriverId { get; set; }

        public int InventoryId { get; set; }
        public double DriverPercentageRate {get;set;} 
        public double DriverFlatRate {get;set;}

        public double quantity { get; set; }
        public double Taxes { get; set; }
        public double TotalAmount { get; set; }
        public double NetAmount { get; set; }
        public double RunningBalance { get; set; }
        
        public double DeliveryFeeCustomer { get; set; }
        public double DeliveryFeeSupplier { get; set; }
        

        [Display(Name = "Status 0 = invalid")]
  
        public int Status { get; set; }

       
       
        
        [Display(Name = "Given to Driver or Recieved from Driver")]
        [DisplayFormat(HtmlEncode = false, ApplyFormatInEditMode = true,DataFormatString="{0:Give/Took}")]
        public bool CreditOrDebit { get; set; }

        public string Notes {get ;set;}
        [Display(Name = "Transaction Type: Debit, Cash, PayPal")]
        public int TransactionType {get;set;}
        public int PartnerId {get;set;}

        

        public Guid CustomerGuid {get;set;}
       public DateTime DeliveryDate {get;set;}

       public DateTime CreatedDate {get;set;}

       
       public DateTime LastChangeDate {get;set;}
       
       public string CreateBy {get;set;}

       public string LastChangeBy {get;set;}
       
    
    }
}