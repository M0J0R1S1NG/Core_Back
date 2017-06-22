using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
namespace Core.Models
{
    public class DebitWay
    {

        public int ID { get; set; }
        
        public string website_unique_id { get; set; }
        public string return_url {get;set;}
        public string transaction_id {get;set;}
        public string merchant_transaction_id {get;set;}
        public string transaction_status {get;set;}
        public string transaction_result {get;set;}
        public DateTime transaction_date {get;set;}
        public string transaction_type {get;set;}

        public string item_name {get;set;}
        public double amount {get;set;}
        public double quantity {get;set;}
        public string item_code {get;set;}
            
        public string language {get;set;}
        public string email {get;set;}
        public string phone {get;set;}
        public string custom {get;set;}
        public string shipment {get;set;}
        
        public string first_name {get;set;}
        public string last_name {get;set;}
        public string address {get;set;}
        public string city {get;set;}
        public string state_or_province {get;set;}
        public string zip_or_postal_code {get;set;}
        public string country {get;set;}

        public string shipping_address {get;set;}
        public string shipping_city {get;set;}
        public string shipping_state_or_province {get;set;}
        public string shipping_zip_or_postal_code {get;set;}
        public string shipping_country {get;set;}

        public string customer_errors_meaning {get;set;}
        public string errors {get;set;}
        public string issuer_name {get;set;}
        public string issuer_confirmation {get;set;}
        
        
        public string status {get;set;}
        public DateTime time_stamp {get;set;}
    }
}