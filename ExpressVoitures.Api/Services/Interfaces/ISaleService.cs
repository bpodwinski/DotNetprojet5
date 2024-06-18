using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Services.Interfaces
{
    /// <summary>
    /// Interface for vehicle service.
    /// </summary>
    public interface ISaleService
    {
        /// <summary>
        /// Adds a new sale.
        /// </summary>
        /// <param name="saleAddDto">The sale data transfer object containing the details of the sale.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when an error occurs while adding the sale.
        /// </exception>
        Task AddSale(SaleAddDto saleAddDto);

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteSale(int id);
    }
}
