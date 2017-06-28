using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Inventory> Inventorys { get; set; }
         public DbSet<InventoryGroup> InventoryGroups { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<MnpForm> MnpForms { get; set; }
        public DbSet<Franchise> Franchise { get; set; }
        
        public DbSet<SMS> SMS { get; set; }
        public DbSet<Partner> Partners { get; set; }
         public DbSet<Comment> Comments { get; set; }
        public DbSet<DeliveryArea> DeliveryAreas { get; set; }
         public DbSet<DeliveryAreaDriver> DeliveryAreaDrivers { get; set; }
         public DbSet<DebitWay> DebitWay { get; set; }
         public DbSet<DebitWayNotification> DebitWayNotifications { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           // builder.Property(p => p.transaction_date).IsRequired().HasColumnName("transaction_date").HasColumnType("datetime2").ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
           // builder.DebitWay(p => p.transaction_date).IsRequired().HasColumnName("transaction_date").HasColumnType("datetime2").ValueGeneratedOnAdd().HasDefaultValueSql("getdate()"); builder.Property(p => 
            // builder.Entity<DebitWay>().Property(p => p.transaction_date).IsRequired(false).HasColumnName("transaction_date").HasColumnType("datetime2").ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
            // builder.Entity<DebitWay>().Property(p => p.transaction_date).IsRequired(false);

            builder.Entity<ApplicationUser>()
            .ToTable("Core_User");
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            
        }

        public DbSet<DeliveryArea> DeliveryArea { get; set; }

        public DbSet<Core.Models.ApplicationUser> ApplicationUser { get; set; }
         
    }
}
