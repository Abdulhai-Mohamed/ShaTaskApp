using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShaTaskApp.Models.Entities;

namespace ShaTaskApp.Models
{
    public class ShaTaskDB : IdentityDbContext<IdentityUser>
    {
        public ShaTaskDB(DbContextOptions<ShaTaskDB> options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cashier> Cashiers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.InvoiceProducts)
                .WithOne() // No navigation property specified on the child side
                .HasForeignKey(ip => ip.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}