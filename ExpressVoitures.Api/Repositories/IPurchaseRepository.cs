using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories
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
    }
}