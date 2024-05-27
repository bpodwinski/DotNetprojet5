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
        Task Add(Sale sale);

        //Task<bool> Update(Sale sale);

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Delete(int id);
    }
}