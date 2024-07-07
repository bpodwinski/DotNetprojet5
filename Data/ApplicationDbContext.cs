using ExpressVoituresV2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoituresV2.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	    public DbSet<ExpressVoituresV2.Models.Vehicle> Vehicle { get; set; } = default!;
	    public DbSet<ExpressVoituresV2.Models.Brand> Brands { get; set; }
	    public DbSet<ExpressVoituresV2.Models.Model> Models { get; set; }
	    public DbSet<ExpressVoituresV2.Models.TrimLevel> TrimLevels { get; set; }
        public DbSet<ExpressVoituresV2.Models.Repair> Repair { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Configuration for Vehicle
            modelBuilder.Entity<Vehicle>(entity =>
            {
                // Numeric precision
                entity.Property(e => e.PurchasePrice)
                      .HasPrecision(18, 2);
                entity.Property(e => e.SalePrice)
                      .HasPrecision(18, 2);

                // Brand relationship
                entity.HasOne(v => v.Brand)
                      .WithMany()
                      .HasForeignKey(v => v.BrandId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Model relationship
                entity.HasOne(v => v.Model)
                      .WithMany()
                      .HasForeignKey(v => v.ModelId)
                      .OnDelete(DeleteBehavior.Restrict);

                // TrimLevel relationship
                entity.HasOne(v => v.TrimLevel)
                      .WithMany()
                      .HasForeignKey(v => v.TrimLevelId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Repairs relationship
                entity.HasMany(v => v.Repairs)
                      .WithOne()
                      .HasForeignKey(r => r.VehicleId);
            });


            // Configuration for Repair
            modelBuilder.Entity<Repair>(entity =>
            {
                // Vehicle relationship
                entity.HasOne(r => r.Vehicle)
                      .WithMany(v => v.Repairs)
                      .HasForeignKey(r => r.VehicleId);

                // Numeric precision
                entity.Property(r => r.Cost)
                      .HasPrecision(18, 2);
            });


            // Configuration for Brand
            modelBuilder.Entity<Brand>(entity =>
            {
                // Models relationship
                entity.HasMany(b => b.Models)
                      .WithOne(m => m.Brand)
                      .HasForeignKey(m => m.BrandId);
            });


            // Configuration for Model
            modelBuilder.Entity<Model>(entity =>
            {
                // TrimLevels relationship
                entity.HasMany(b => b.TrimLevels)
                      .WithOne(m => m.Model)
                      .HasForeignKey(m => m.ModelId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Brands relationship
                entity.HasOne(m => m.Brand)
                      .WithMany(b => b.Models)
                      .HasForeignKey(m => m.BrandId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // Configuration for TrimLevel
            modelBuilder.Entity<TrimLevel>(entity =>
            {
                // TrimLevels relationship
                entity.HasOne(t => t.Model)
                      .WithMany(m => m.TrimLevels)
                      .HasForeignKey(t => t.ModelId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
