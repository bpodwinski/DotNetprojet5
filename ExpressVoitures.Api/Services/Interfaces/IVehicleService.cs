using ExpressVoituresApi.Models.Dtos;

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

        Task AddVehicles(IEnumerable<VehicleAddDto> vehicleAddDto);

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
    }
}