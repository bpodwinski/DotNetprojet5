using ExpressVoituresApi.Models.Dtos;

namespace ExpressVoituresApi.Services.Interfaces
{
    /// <summary>
    /// Interface for vehicle service.
    /// </summary>
    public interface IRepairService
    {
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
    }
}
