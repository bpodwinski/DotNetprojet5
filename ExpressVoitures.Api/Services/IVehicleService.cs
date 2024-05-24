using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Services
{
    /// <summary>
    /// Interface for vehicle service.
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Retrieves a list of vehicles with optional pagination, filtering, and sorting.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="brand">Optional filter by brand.</param>
        /// <param name="sortOrder">Optional sort order.</param>
        /// <returns>A list of vehicle DTOs.</returns>
        Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int pageNumber, int pageSize, string brand, string sortOrder);

        /// <summary>
        /// Retrieves a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle DTO with the specified ID, or null if not found.</returns>
        Task<VehicleDto> GetVehicleByIdAsync(int id);

        /// <summary>
        /// Retrieves a vehicle by ID with its purchase information.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with purchase DTO with the specified ID, or null if not found.</returns>
        Task<VehicleWithPurchaseDto> GetVehicleWithPurchaseByIdAsync(int id);

        /// <summary>
        /// Adds a new purchase to a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddPurchaseToVehicleAsync(int vehicleId, PurchaseDto purchaseDto);

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicleDto">The vehicle data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddVehicleAsync(VehicleDto vehicleDto);

        /// <summary>
        /// Updates a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to update.</param>
        /// <param name="vehicle">The updated vehicle entity.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> UpdateVehicleAsync(int id, Vehicle vehicle);

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteVehicleAsync(int id);
    }
}
