using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using System.Threading.Tasks;

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
        public async Task Add(Purchase purchase)
        {
            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a purchase by ID.
        /// </summary>
        /// <param name="purchaseId">The ID of the purchase to retrieve.</param>
        /// <returns>The purchase entity with the specified ID, or null if not found.</returns>
        public async Task<Purchase> GetById(int purchaseId)
        {
            return await _context.Purchases.FindAsync(purchaseId);
        }

        /// <summary>
        /// Deletes a purchase by ID.
        /// </summary>
        /// <param name="purchaseId">The ID of the purchase to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Delete(int purchaseId)
        {
            var purchase = await GetById(purchaseId);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();
            }
        }
    }
}