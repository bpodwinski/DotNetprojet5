using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services.Interfaces;

namespace ExpressVoituresApi.Services
{
    /// <summary>
    /// Service for managing vehicles.
    /// </summary>
    public class PurchaseService : IPurchaseService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ILogger<VehicleService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        /// <param name="vehicleRepository">The vehicle repository.</param>
        /// <param name="purchaseRepository">The purchase repository.</param>
        /// <param name="logger">The logger.</param>
        public PurchaseService(
            IVehicleRepository vehicleRepository,
            IPurchaseRepository purchaseRepository,
            ILogger<VehicleService> logger
            )
        {
            _vehicleRepository = vehicleRepository;
            _purchaseRepository = purchaseRepository;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new purchase to a vehicle
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the vehicle is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the vehicle already has a purchase.</exception>
        public async Task AddPurchase(int vehicleId, PurchaseDto purchaseDto)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetById(vehicleId);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found");
                }

                if (vehicle.purchase != null)
                {
                    throw new InvalidOperationException("The vehicle already has a purchase");
                }

                var purchase = new Model
                {
                    date = purchaseDto.date,
                    price = purchaseDto.price,
                    vehicle_id = vehicleId
                };

                await _purchaseRepository.Add(purchase);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        /// <summary>
        /// Updates an existing purchase of a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the vehicle or purchase is not found.</exception>
        public async Task UpdatePurchase(int vehicleId, PurchaseDto purchaseDto)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetById(vehicleId);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found");
                }

                if (vehicle.purchase == null)
                {
                    throw new InvalidOperationException($"No purchase found for Vehicle ID {vehicleId}");
                }

                var purchase = vehicle.purchase;
                purchase.date = purchaseDto.date;
                purchase.price = purchaseDto.price;

                await _purchaseRepository.Update(purchase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the purchase");
                throw new InvalidOperationException("An error occurred while updating the purchase");
            }
        }

        /// <summary>
        /// Deletes the purchase associated with a vehicle by vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle whose purchase will be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the vehicle with the specified ID is not found,
        /// or when no purchase is found for the specified vehicle.
        /// </exception>
        public async Task DeletePurchase(int vehicleId)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetById(vehicleId);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found");
                }

                if (vehicle.purchase == null)
                {
                    throw new InvalidOperationException($"No purchase found for Vehicle ID {vehicleId}");
                }

                await _purchaseRepository.Delete(vehicle.purchase.id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting purchase with ID {vehicleId}");
                throw new InvalidOperationException("An error occurred while deleting the purchase");
            }
        }
    }
}
