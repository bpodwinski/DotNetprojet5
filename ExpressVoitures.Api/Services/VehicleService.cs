using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoituresApi.Services
{
    /// <summary>
    /// Service for managing vehicles.
    /// </summary>
    public class VehicleService : IVehicleService
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
        public VehicleService(
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
        /// Retrieves a list of vehicles with optional pagination, filtering, and sorting.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="brand">Optional filter by brand.</param>
        /// <param name="sortOrder">Optional sort order.</param>
        /// <returns>A list of vehicle DTOs.</returns>
        public async Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int pageNumber, int pageSize, string brand, string sortOrder)
        {
            try
            {
                var query = _vehicleRepository.GetAll();
                if (!string.IsNullOrEmpty(brand))
                {
                    query = query.Where(v => v.Brand.Contains(brand));
                }

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    query = sortOrder.ToLower() switch
                    {
                        "brand" => query.OrderBy(v => v.Brand),
                        "model" => query.OrderBy(v => v.Model),
                        "year" => query.OrderBy(v => v.Year),
                        "id" => query.OrderBy(v => v.Id),
                        _ => query.OrderBy(v => v.Id),
                    };
                }

                var vehicles = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(v => new VehicleDto
                    {
                        Id = v.Id,
                        CreateDate = v.CreateDate,
                        Vin = v.Vin,
                        Year = v.Year,
                        Brand = v.Brand,
                        Model = v.Model,
                        TrimLevel = v.TrimLevel
                    })
                    .ToListAsync();

                return vehicles;

            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        /// <summary>
        /// Retrieves a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle DTO with the specified ID, or null if not found.</returns>
        public async Task<VehicleDto> GetVehicleByIdAsync(int id)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(id);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle ID {id} not found");
                }

                return new VehicleDto
                {
                    Id = vehicle.Id,
                    CreateDate = vehicle.CreateDate,
                    Vin = vehicle.Vin,
                    Year = vehicle.Year,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    TrimLevel = vehicle.TrimLevel
                };
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        /// <summary>
        /// Retrieves a vehicle by ID with its purchase information
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with purchase DTO with the specified ID, or null if not found.</returns>
        public async Task<VehicleWithPurchaseDto> GetVehicleWithPurchaseByIdAsync(int id)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetByIdWithPurchaseAsync(id);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle ID {id} not found");
                }

                return new VehicleWithPurchaseDto
                {
                    Id = vehicle.Id,
                    CreateDate = vehicle.CreateDate,
                    Vin = vehicle.Vin,
                    Year = vehicle.Year,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    TrimLevel = vehicle.TrimLevel,
                    Purchase = vehicle.Purchase != null ? new PurchaseDto
                    {
                        Id = vehicle.Purchase.Id,
                        Date = vehicle.Purchase.Date,
                        Price = vehicle.Purchase.Price,
                    } : null
                };
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        /// <summary>
        /// Adds a new vehicle
        /// </summary>
        /// <param name="vehicleDto">The vehicle data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while adding the vehicle.</exception>
        public async Task AddVehicleAsync(VehicleDto vehicleDto)
        {
            try
            {
                var vehicle = new Vehicle
                {
                    Id = vehicleDto.Id,
                    CreateDate = vehicleDto.CreateDate,
                    Vin = vehicleDto.Vin,
                    Year = vehicleDto.Year,
                    Brand = vehicleDto.Brand,
                    Model = vehicleDto.Model,
                    TrimLevel = vehicleDto.TrimLevel
                };

                await _vehicleRepository.AddAsync(vehicle);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        /// <summary>
        /// Adds a new purchase to a vehicle
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the vehicle is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the vehicle already has a purchase.</exception>
        public async Task AddPurchaseToVehicleAsync(int vehicleId, PurchaseDto purchaseDto)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetByIdWithPurchaseAsync(vehicleId);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found");
                }

                if (vehicle.Purchase != null)
                {
                    throw new InvalidOperationException("The vehicle already has a purchase");
                }

                var purchase = new Purchase
                {
                    Date = purchaseDto.Date,
                    Price = purchaseDto.Price,
                    VehicleId = vehicleId
                };

                await _purchaseRepository.AddAsync(purchase);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        /// <summary>
        /// Updates an existing vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to update.</param>
        /// <param name="vehicleDto">The updated vehicle entity.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public async Task<bool> UpdateVehicleAsync(int id, VehicleDto vehicleDto)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetByIdAsync(id);
                if (existingVehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle ID {id} not found");
                }

                var vehicle = new Vehicle
                {
                    Id = id,
                    Vin = vehicleDto.Vin,
                    Year = vehicleDto.Year,
                    Brand = vehicleDto.Brand,
                    Model = vehicleDto.Model,
                    TrimLevel = vehicleDto.TrimLevel
                };

                return await _vehicleRepository.UpdateAsync(vehicle);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteVehicleAsync(int id)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetByIdAsync(id);
                if (existingVehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle ID {id} not found");
                }

                await _vehicleRepository.DeleteAsync(id);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }
    }
}