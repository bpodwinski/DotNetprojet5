using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface for repair repository.
    /// </summary>
    public interface IRepairRepository
    {
        /// <summary>
        /// Adds a new repair.
        /// </summary>
        /// <param name="repair">The repair entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(Repair repair);

        /// <summary>
        /// Updates an existing repair.
        /// </summary>
        /// <param name="repair">The repair entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(Repair repair);

        /// <summary>
        /// Deletes a repair by its ID.
        /// </summary>
        /// <param name="id">The ID of the repair to delete.</param>
        /// <returns>A task representing the asynchronous operation with a boolean result indicating success.</returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Gets a repair by its ID.
        /// </summary>
        /// <param name="id">The ID of the repair to retrieve.</param>
        /// <returns>The repair entity or null if not found.</returns>
        Task<Repair?> GetById(int id);
    }
}
