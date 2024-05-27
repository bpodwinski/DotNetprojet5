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

        //Task<bool> Update(Purchase purchase);

        /// <summary>
        /// Deletes a purchase by ID.
        /// </summary>
        /// <param name="id">The ID of the purchase to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Delete(int id);
    }
}