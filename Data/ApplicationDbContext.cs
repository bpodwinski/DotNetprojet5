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
	}
}
