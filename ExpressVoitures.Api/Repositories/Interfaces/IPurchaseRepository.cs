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
        Task Add(Purchase purchase);

        /// <summary>
        /// Updates an existing purchase.
        /// </summary>
        /// <param name="purchase">The purchase entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(Purchase purchase);

        /// <summary>
        /// Gets a purchase by its ID.
        /// </summary>
        /// <param name="id">The ID of the purchase to retrieve.</param>
        /// <returns>The purchase entity or null if not found.</returns>
        Task<Purchase?> GetById(int id);

        /// <summary>
        /// Deletes a purchase by ID.
        /// </summary>
        /// <param name="id">The ID of the purchase to delete.</param>
        /// <returns>The deleted purchase entity or null if not found.</returns>
        Task<bool> Delete(int id);
    }
}
