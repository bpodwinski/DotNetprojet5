using Microsoft.EntityFrameworkCore;
using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Data
{
    /// <summary>
    /// Application database context for ExpressVoitures API.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        /// <summary>
        /// Gets or sets the Vehicles DbSet.
        /// </summary>
        public DbSet<Vehicle> Vehicles { get; set; }

        /// <summary>
        /// Gets or sets the Purchases DbSet.
        /// </summary>
        public DbSet<Purchase> Purchases { get; set; }

        /// <summary>
        /// Configures the schema needed for the application.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set default value for Vehicle.CreateDate
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            // Set default value for Purchase.CreateDate
            modelBuilder.Entity<Purchase>()
                .Property(p => p.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            // Configure one-to-one relationship between Vehicle and Purchase
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Purchase)
                .WithOne(p => p.Vehicle)
                .HasForeignKey<Purchase>(p => p.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
