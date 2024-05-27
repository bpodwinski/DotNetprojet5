using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Services.Interfaces
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
        Task<IEnumerable<VehicleDto>> GetAllVehicles(int pageNumber, int pageSize, string brand, string sortOrder);

        /// <summary>
        /// Retrieves a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle DTO with the specified ID, or null if not found.</returns>
        Task<VehicleDto> GetVehicleById(int id);

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicleAddDto">The vehicle data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddVehicle(VehicleAddDto vehicleAddDto);

        /// <summary>
        /// Adds a new purchase to a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddPurchase(int vehicleId, PurchaseDto purchaseDto);

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
        /// Adds a new repair to a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to add the repair to.</param>
        /// <param name="repairAddDto">The repair data transfer object containing the details of the repair.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the vehicle with the specified ID is not found,
        /// or when an error occurs while adding the repair.
        /// </exception>
        Task AddRepair(int vehicleId, RepairAddDto repairAddDto);

        /// <summary>
        /// Updates a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to update.</param>
        /// <param name="vehicleAddDto">The updated vehicle entity.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> UpdateVehicle(int id, VehicleAddDto vehicleAddDto);

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteVehicle(int id);

        /// <summary>
        /// Deletes a purchase by ID.
        /// </summary>
        /// <param name="id">The ID of the purchase to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeletePurchase(int id);

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteSale(int id);
    }
}