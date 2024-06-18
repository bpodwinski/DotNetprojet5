using ExpressVoituresApi.Models.Dtos;

namespace ExpressVoituresApi.Services.Interfaces
{
    /// <summary>
    /// Interface for vehicle service.
    /// </summary>
    public interface IPurchaseService
    {
        /// <summary>
        /// Adds a new purchase to a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddPurchase(int vehicleId, PurchaseDto purchaseDto);

        Task UpdatePurchase(int vehicleId, PurchaseDto purchaseDto);

        /// <summary>
        /// Deletes a purchase by ID.
        /// </summary>
        /// <param name="id">The ID of the purchase to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeletePurchase(int id);
    }
}
