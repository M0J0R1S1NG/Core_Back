
using Core.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public static class DbInitializer
    {
        
        public static void Initialize(ApplicationDbContext  context)
        {
          
           context.Database.EnsureCreated();
            //context.Database.Migrate();
            
            // Look for any data.
        //     if (context.Orders.Any())
        //     {
        //         return;   // DB has been seeded
        //     }

        //     var orders = new Order[]
        //     {
        //     //new Order{CustomerId=1,OrderDate=DateTime.Now ,DeliveryDate=DateTime.Now,Total=70,Weight=7},
        //    //new Order{CustomerId=1,OrderDate=DateTime.Now ,DeliveryDate=DateTime.Now,Total=35,Weight=3},

        //     };
        //     foreach (Order s in orders)
        //     {
        //         context.Orders.Add(s);
        //     }
        //     context.SaveChanges();
            return;
        }
    }
}