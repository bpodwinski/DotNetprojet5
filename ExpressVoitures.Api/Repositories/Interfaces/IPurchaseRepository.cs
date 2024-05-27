using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface for purchase repository.
    /// </summary>
    public interface IPurchaseRepository
    {
        /// <summary>
        /// Adds a new purchase.
        /// </summary>
        /// <param name="purchase">The purchase entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Purchase purchase);

        /// <summary>
        /// Retrieves a purchase by ID.
        /// </summary>
        /// <param name="purchaseId">The ID of the purchase to retrieve.</param>
        /// <returns>The purchase entity with the specified ID, or null if not found.</returns>
        Task<Purchase> GetByIdAsync(int purchaseId);

        /// <summary>
        /// Deletes a purchase by ID.
        /// </summary>
        /// <param name="purchaseId">The ID of the purchase to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int purchaseId);
    }
}