using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoituresApi.Services
{
    /// <summary>
    /// Service for managing vehicles.
    /// </summary>
    public class RepairService : IRepairService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRepairRepository _repairRepository;
        private readonly ILogger<RepairService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepairService"/> class.
        /// </summary>
        /// <param name="vehicleRepository">The vehicle repository.</param>
        /// <param name="repairRepository">The repair repository.</param>
        /// <param name="logger">The logger.</param>
        public RepairService(
            IVehicleRepository vehicleRepository,
            IRepairRepository repairRepository,
            ILogger<RepairService> logger
            )
        {
            _vehicleRepository = vehicleRepository;
            _repairRepository = repairRepository;
            _logger = logger;
        }

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
        public async Task AddRepair(int vehicleId, RepairAddDto repairAddDto)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetById(vehicleId);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found");
                }

                var repair = new Repair
                {
                    vehicle_id = vehicleId,
                    description = repairAddDto.description,
                    cost = repairAddDto.cost
                };

                await _repairRepository.Add(repair);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding a repair to vehicle with ID {vehicleId}");
                throw new InvalidOperationException("An error occurred while adding the repair");
            }
        }
    }
}
