using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrlandoServices.Core.Models;


namespace OrlandoServices.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<User> User { get; set; } = null!;
        public DbSet<Service> Service { get; set; } = null!;
        public DbSet<ServiceField> ServiceField { get; set; } = null!;
        public DbSet<Order> Order { get; set; } = null!;
        public DbSet<OrderFieldValue> OrderFieldValue { get; set; } = null!;
        public DbSet<Payment> Payment { get; set; } = null!;
        public DbSet<Donation> Donation { get; set; } = null!;
        public DbSet<AdminUser> AdminUser { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // קשרים בין מודלים

            // User → Orders (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Service → Orders (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Service)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Service → ServiceFields (One-to-Many)
            modelBuilder.Entity<ServiceField>()
                .HasOne(sf => sf.Service)
                .WithMany(s => s.ServiceFields)
                .HasForeignKey(sf => sf.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Orders → OrderFieldValues (One-to-Many)
            modelBuilder.Entity<OrderFieldValue>()
                .HasOne(ofv => ofv.Order)
                .WithMany(o => o.OrderFieldValues)
                .HasForeignKey(ofv => ofv.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ServiceFields → OrderFieldValues (One-to-Many)
            modelBuilder.Entity<OrderFieldValue>()
                .HasOne(ofv => ofv.ServiceField)
                .WithMany(sf => sf.OrderFieldValues)
                .HasForeignKey(ofv => ofv.ServiceFieldId)
                .OnDelete(DeleteBehavior.Restrict);

            // Orders → Payments (One-to-Many)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Donations (One-to-Many, optional)
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.User)
                .WithMany(o => o.Donations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Orders → Donations (One-to-Many, optional)
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Order)
                .WithMany(o => o.Donations)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}