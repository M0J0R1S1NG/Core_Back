using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Comment
    {

        public int ID { get; set; }
       
        public Guid CreatedBy { get; set; }

        public string Text {get;set;}
        public int Rating {get;set;}
        public int Likes {get;set;}
        public int DisLikes {get;set;} 
        public int Status { get; set; }
       
    }
}
