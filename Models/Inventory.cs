using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Inventory
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public bool OnHand { get; set; }
        public DateTime BestBefore { get; set; }
        public DateTime OrderDate { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public string Supplier { get; set; }
        public string Notes { get; set; }
        public Byte[] Photo { get; set; }
        public double THCContent { get; set; }
        public double CBDContent { get; set; }
        public int Likes { get; set; }

    }
}
