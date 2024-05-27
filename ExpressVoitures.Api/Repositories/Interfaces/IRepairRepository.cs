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

        //Task<bool> Update(Repair repair);

        /// <summary>
        /// Deletes a repair by its ID.
        /// </summary>
        /// <param name="id">The ID of the repair to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        //Task Delete(int id);
    }
}