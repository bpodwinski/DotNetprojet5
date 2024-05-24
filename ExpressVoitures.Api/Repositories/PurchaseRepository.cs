using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories
{
    /// <summary>
    /// Repository for managing purchase entities.
    /// </summary>
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public PurchaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new purchase.
        /// </summary>
        /// <param name="purchase">The purchase entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(Purchase purchase)
        {
            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();
        }
    }
}
