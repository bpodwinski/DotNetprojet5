using Microsoft.EntityFrameworkCore;
using Express_Voitures.Models.Entities;

namespace Express_Voitures.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.CreateDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}