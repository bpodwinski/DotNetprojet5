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
        public DbSet<Model> Purchases { get; set; }

        /// <summary>
        /// Gets or sets the Repairs DbSet.
        /// </summary>
        public DbSet<Repair> Repairs { get; set; }

        /// <summary>
        /// Gets or sets the Sales DbSet.
        /// </summary>
        public DbSet<Brand> Sales { get; set; }

        /// <summary>
        /// Gets or sets the UserAccounts DbSet.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Configures the schema needed for the application.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set default value for User.create_date
            modelBuilder.Entity<User>()
                .Property(v => v.create_date)
                .HasDefaultValueSql("GETDATE()");

            // Set default value for Vehicle.create_date
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.create_date)
                .HasDefaultValueSql("GETDATE()");

            // Set default value for Purchase.date
            modelBuilder.Entity<Model>()
                .Property(p => p.date)
                .HasDefaultValueSql("GETDATE()");

            // Set default value for Repair.create_date
            modelBuilder.Entity<Repair>()
                .Property(r => r.create_date)
                .HasDefaultValueSql("GETDATE()");

            // Set default value for Sale.create_date
            modelBuilder.Entity<Brand>()
                .Property(s => s.create_date)
                .HasDefaultValueSql("GETDATE()");

            // Configure one-to-one relationship between Vehicle and Purchase
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.purchase)
                .WithOne(p => p.vehicle)
                .HasForeignKey<Model>(p => p.vehicle_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one relationship between Vehicle and Sale
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.sale)
                .WithOne(s => s.vehicle)
                .HasForeignKey<Brand>(s => s.vehicle_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Vehicle and Repairs
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.repair)
                .WithOne(r => r.vehicle)
                .HasForeignKey(r => r.vehicle_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}