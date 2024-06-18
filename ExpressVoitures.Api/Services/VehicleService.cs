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
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILogger<VehicleService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        /// <param name="vehicleRepository">The vehicle repository.</param>
        /// <param name="logger">The logger.</param>
        public VehicleService(
            IVehicleRepository vehicleRepository,
            ILogger<VehicleService> logger
            )
        {
            _vehicleRepository = vehicleRepository;
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
        public async Task<IEnumerable<VehicleDto>> GetAllVehicles(int pageNumber, int pageSize, string brand, string sortOrder)
        {
            try
            {
                var query = _vehicleRepository.GetAll();

                if (!string.IsNullOrEmpty(brand))
                {
                    query = query.Where(v => v.brand.Contains(brand));
                }

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    query = sortOrder.ToLower() switch
                    {
                        "brand" => query.OrderBy(vehicle => vehicle.brand),
                        "model" => query.OrderBy(vehicle => vehicle.model),
                        "year" => query.OrderBy(vehicle => vehicle.year),
                        "id" => query.OrderBy(vehicle => vehicle.id),
                        _ => query.OrderBy(vehicle => vehicle.id),
                    };
                }

                query = query
                    .Include(v => v.purchase)
                    .Include(v => v.sale)
                    .Include(v => v.repair);

                var vehicles = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(vehicle => new VehicleDto
                    {
                        id = vehicle.id,
                        create_date = vehicle.create_date,
                        vin = vehicle.vin,
                        year = vehicle.year,
                        brand = vehicle.brand,
                        model = vehicle.model,
                        trim_level = vehicle.trim_level,
                        purchase = vehicle.purchase == null ? null : new PurchaseDto
                        {
                            id = vehicle.purchase.id,
                            vehicle_id = vehicle.id,
                            date = vehicle.purchase.date,
                            price = vehicle.purchase.price
                        },
                        sale = vehicle.sale == null ? null : new SaleDto
                        {
                            id = vehicle.sale.id,
                            vehicle_id = vehicle.id,
                            create_date = vehicle.sale.create_date,
                            availability_date = vehicle.sale.availability_date,
                            sale_date = vehicle.sale.sale_date,
                            price = vehicle.sale.price,
                            title = vehicle.sale.title,
                            description = vehicle.sale.description
                        },
                        repair = vehicle.repair.Select(repair => new RepairDto
                        {
                            id = repair.id,
                            vehicle_id = vehicle.id,
                            create_date = repair.create_date,
                            description = repair.description,
                            cost = repair.cost
                        }).ToList()
                    })
                    .ToListAsync();

                return vehicles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving vehicles");
                throw new InvalidOperationException("An error occurred while retrieving vehicles", ex);
            }
        }

        /// <summary>
        /// Retrieves a vehicle by ID with its purchase, sale, and repair information
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle DTO with the specified ID, or null if not found.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the vehicle with the specified ID is not found, 
        /// or when an error occurs while retrieving the vehicle.
        /// </exception>
        public async Task<VehicleDto> GetVehicleById(int id)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetAll()
                    .Include(v => v.purchase)
                    .Include(v => v.sale)
                    .Include(v => v.repair)
                    .FirstOrDefaultAsync(v => v.id == id);

                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle ID {id} not found");
                }

                return new VehicleDto
                {
                    id = vehicle.id,
                    create_date = vehicle.create_date,
                    vin = vehicle.vin,
                    year = vehicle.year,
                    brand = vehicle.brand,
                    model = vehicle.model,
                    trim_level = vehicle.trim_level,
                    purchase = vehicle.purchase == null ? null : new PurchaseDto
                    {
                        id = vehicle.purchase.id,
                        vehicle_id = vehicle.id,
                        date = vehicle.purchase.date,
                        price = vehicle.purchase.price
                    },
                    sale = vehicle.sale == null ? null : new SaleDto
                    {
                        id = vehicle.sale.id,
                        vehicle_id = vehicle.id,
                        create_date = vehicle.sale.create_date,
                        availability_date = vehicle.sale.availability_date,
                        sale_date = vehicle.sale.sale_date,
                        price = vehicle.sale.price,
                        title = vehicle.sale.title,
                        description = vehicle.sale.description
                    },
                    repair = vehicle.repair.Select(repair => new RepairDto
                    {
                        id = repair.id,
                        vehicle_id = vehicle.id,
                        create_date = repair.create_date,
                        description = repair.description,
                        cost = repair.cost
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving vehicle with ID {id}");
                throw new InvalidOperationException("An error occurred while retrieving the vehicle", ex);
            }
        }

        /// <summary>
        /// Adds a new vehicle
        /// </summary>
        /// <param name="vehicleAddDto">The vehicle data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while adding the vehicle.</exception>
        public async Task AddVehicle(VehicleAddDto vehicleAddDto)
        {
            try
            {
                var vehicle = new Vehicle
                {
                    id = vehicleAddDto.id,
                    vin = vehicleAddDto.vin,
                    year = vehicleAddDto.year,
                    brand = vehicleAddDto.brand,
                    model = vehicleAddDto.model,
                    trim_level = vehicleAddDto.trim_level
                };

                await _vehicleRepository.Add(vehicle);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        public async Task AddVehicles(IEnumerable<VehicleAddDto> vehicleAddDto)
        {
            var vehicleList = vehicleAddDto.Select(dto => new Vehicle
            {
                id = dto.id,
                vin = dto.vin,
                year = dto.year,
                brand = dto.brand,
                model = dto.model,
                trim_level = dto.trim_level
            }).ToList();

            await _vehicleRepository.AddBulk(vehicleList);
        }

        /// <summary>
        /// Updates an existing vehicle by ID
        /// </summary>
        /// <param name="id">The ID of the vehicle to update.</param>
        /// <param name="vehicleAddDto">The updated vehicle entity.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public async Task<bool> UpdateVehicle(int id, VehicleAddDto vehicleAddDto)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetById(id);
                if (existingVehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle ID {id} not found");
                }

                existingVehicle.vin = vehicleAddDto.vin;
                existingVehicle.year = vehicleAddDto.year;
                existingVehicle.brand = vehicleAddDto.brand;
                existingVehicle.model = vehicleAddDto.model;
                existingVehicle.trim_level = vehicleAddDto.trim_level;

                return await _vehicleRepository.Update(existingVehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving vehicles");
                throw new InvalidOperationException("An error occurred while retrieving vehicles", ex);
            }
        }

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteVehicle(int id)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetById(id);
                if (existingVehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle ID {id} not found");
                }

                await _vehicleRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the vehicle");
                throw new InvalidOperationException("An error occurred while deleting the vehicle", ex);
            }
        }
    }
}