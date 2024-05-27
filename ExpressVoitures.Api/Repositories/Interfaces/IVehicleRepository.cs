using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface for vehicle repository.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Retrieves all vehicles.
        /// </summary>
        /// <returns>An IQueryable of vehicles.</returns>
        IQueryable<Vehicle> GetAll();

        /// <summary>
        /// Retrieves a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with the specified ID, or null if not found.</returns>
        Task<Vehicle?> GetById(int id);

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(Vehicle vehicle);

        /// <summary>
        /// Updates an existing vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to update.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> Update(Vehicle vehicle);

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Delete(int id);
    }
}