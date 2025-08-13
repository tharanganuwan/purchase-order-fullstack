using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using PurchaseOrder.Api.Entities;

namespace PurchaseOrder.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<PurchaseOrders> PurchaseOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<PurchaseOrders>()
              .HasIndex(p => p.PoNumber)
              .IsUnique();

            mb.Entity<PurchaseOrders>()
              .Property(p => p.TotalAmount)
              .HasPrecision(18, 2);


        }
    }
}
