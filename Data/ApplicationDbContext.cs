using ExpressVoituresV2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpressVoituresV2.ViewModel;

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

	    protected override void OnModelCreating(ModelBuilder modelBuilder)
	    {
		    base.OnModelCreating(modelBuilder);

		    modelBuilder.Entity<Vehicle>()
			    .HasOne(v => v.Brand)
			    .WithMany()
			    .HasForeignKey(v => v.BrandId)
			    .OnDelete(DeleteBehavior.Restrict);

		    modelBuilder.Entity<Vehicle>()
			    .HasOne(v => v.Model)
			    .WithMany()
			    .HasForeignKey(v => v.ModelId)
			    .OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Vehicle>()
			    .HasOne(v => v.TrimLevel)
			    .WithMany()
			    .HasForeignKey(v => v.TrimLevelId);

			modelBuilder.Entity<Brand>()
			    .HasMany(b => b.Models)
			    .WithOne(m => m.Brand)
			    .HasForeignKey(m => m.BrandId);
	    }
	}
}
