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
        public async Task Add(Model purchase)
        {
            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing purchase.
        /// </summary>
        /// <param name="purchase">The purchase entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Update(Model purchase)
        {
            _context.Purchases.Update(purchase);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a purchase by its ID.
        /// </summary>
        /// <param name="id">The ID of the purchase to retrieve.</param>
        /// <returns>The purchase entity or null if not found.</returns>
        public async Task<Model?> GetById(int id)
        {
            return await _context.Purchases.FindAsync(id);
        }

        /// <summary>
        /// Deletes a purchase by its ID.
        /// </summary>
        /// <param name="id">The ID of the purchase to delete.</param>
        /// <returns>The deleted purchase entity or null if not found.</returns>
        public async Task<bool> Delete(int id)
        {
            var purchase = await GetById(id);
            if (purchase == null)
            {
                return false;
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
