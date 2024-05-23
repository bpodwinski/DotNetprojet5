using Microsoft.EntityFrameworkCore;
using Express_Voitures.Models.Entities;

namespace Express_Voitures.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Purchase>()
                .Property(p => p.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Purchase)
                .WithOne(p => p.Vehicle)
                .HasForeignKey<Purchase>(p => p.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}