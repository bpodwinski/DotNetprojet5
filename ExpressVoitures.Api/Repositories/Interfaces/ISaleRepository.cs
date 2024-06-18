using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface for sale repository.
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Adds a new sale.
        /// </summary>
        /// <param name="sale">The sale entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(Brand sale);

        /// <summary>
        /// Updates an existing sale.
        /// </summary>
        /// <param name="sale">The sale entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(Brand sale);

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <returns>A task representing the asynchronous operation with a boolean result indicating success.</returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Gets a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to retrieve.</param>
        /// <returns>The sale entity or null if not found.</returns>
        Task<Brand?> GetById(int id);
    }
}
