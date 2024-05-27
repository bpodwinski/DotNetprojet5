using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories;
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
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IRepairRepository _repairRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<VehicleService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        /// <param name="vehicleRepository">The vehicle repository.</param>
        /// <param name="purchaseRepository">The purchase repository.</param>
        /// <param name="repairRepository">The repair repository.</param>
        /// <param name="saleRepository">The sale repository.</param>
        /// <param name="logger">The logger.</param>
        public VehicleService(
            IVehicleRepository vehicleRepository,
            IPurchaseRepository purchaseRepository,
            IRepairRepository repairRepository,
            ISaleRepository saleRepository,
            ILogger<VehicleService> logger
            )
        {
            _vehicleRepository = vehicleRepository;
            _purchaseRepository = purchaseRepository;
            _repairRepository = repairRepository;
            _saleRepository = saleRepository;
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
        public async Task<VehicleDto> GetVehicleByIdAsync(int id)
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
        /// Retrieves a vehicle by ID with its purchase information
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with purchase DTO with the specified ID, or null if not found.</returns>
        public async Task<VehicleDto> GetVehicleWithPurchaseByIdAsync(int id)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetByIdWithPurchaseAsync(id);
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
                        date = vehicle.purchase.date,
                        price = vehicle.purchase.price,
                    }
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
        /// <param name="vehicleAddDto">The vehicle data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while adding the vehicle.</exception>
        public async Task AddVehicleAsync(VehicleAddDto vehicleAddDto)
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

                if (vehicle.purchase != null)
                {
                    throw new InvalidOperationException("The vehicle already has a purchase");
                }

                var purchase = new Purchase
                {
                    date = purchaseDto.date,
                    price = purchaseDto.price,
                    vehicle_id = vehicleId
                };

                await _purchaseRepository.AddAsync(purchase);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("An error occurred while updating the vehicle");
            }
        }

        /// <summary>
        /// Updates an existing vehicle by ID
        /// </summary>
        /// <param name="id">The ID of the vehicle to update.</param>
        /// <param name="vehicleAddDto">The updated vehicle entity.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public async Task<bool> UpdateVehicleAsync(int id, VehicleAddDto vehicleAddDto)
        {
            try
            {
                var existingVehicle = await _vehicleRepository.GetByIdAsync(id);
                if (existingVehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle ID {id} not found");
                }

                existingVehicle.vin = vehicleAddDto.vin;
                existingVehicle.year = vehicleAddDto.year;
                existingVehicle.brand = vehicleAddDto.brand;
                existingVehicle.model = vehicleAddDto.model;
                existingVehicle.trim_level = vehicleAddDto.trim_level;

                return await _vehicleRepository.UpdateAsync(existingVehicle);
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

        /// <summary>
        /// Deletes the purchase associated with a vehicle by vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle whose purchase will be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the vehicle with the specified ID is not found,
        /// or when no purchase is found for the specified vehicle.
        /// </exception>
        public async Task DeletePurchaseByVehicleIdAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetByIdWithPurchaseAsync(vehicleId);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found");
                }

                if (vehicle.purchase == null)
                {
                    throw new InvalidOperationException($"No purchase found for Vehicle ID {vehicleId}");
                }

                await _purchaseRepository.DeleteAsync(vehicle.purchase.id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting purchase with ID {vehicleId}");
                throw new InvalidOperationException("An error occurred while deleting the purchase");
            }
        }

        public async Task AddRepairToVehicleAsync(int vehicleId, RepairAddDto repairAddDto)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
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

                await _repairRepository.AddAsync(repair);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding a repair to vehicle with ID {vehicleId}");
                throw new InvalidOperationException("An error occurred while adding the repair");
            }
        }

        public async Task AddSaleAsync(SaleAddDto saleAddDto)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(saleAddDto.vehicle_id);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {saleAddDto.vehicle_id} not found");
                }

                if (vehicle.sale != null)
                {
                    throw new InvalidOperationException("The vehicle already has a sale");
                }

                var sale = new Sale
                {
                    vehicle_id = saleAddDto.vehicle_id,
                    create_date = saleAddDto.create_date,
                    availability_date = saleAddDto.availability_date,
                    sale_date = saleAddDto.sale_date,
                    price = saleAddDto.price,
                    title = saleAddDto.title,
                    description = saleAddDto.description
                };

                await _saleRepository.AddAsync(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding a sale to vehicle with ID {saleAddDto.vehicle_id}");
                throw new InvalidOperationException("An error occurred while adding the sale");
            }
        }

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="saleId">The ID of the sale to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteSaleAsync(int saleId)
        {
            await _saleRepository.DeleteAsync(saleId);
        }

        /// <summary>
        /// Retrieves a sale by its ID.
        /// </summary>
        /// <param name="saleId">The ID of the sale to retrieve.</param>
        /// <returns>The sale DTO with the specified ID, or null if not found.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the sale with the specified ID is not found.
        /// </exception>
        public async Task<SaleDto> GetSaleByIdAsync(int saleId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null)
            {
                throw new InvalidOperationException($"Sale with ID {saleId} not found");
            }

            return new SaleDto
            {
                id = sale.id,
                create_date = sale.create_date,
                availability_date = sale.availability_date,
                sale_date = sale.sale_date,
                price = sale.price,
                title = sale.title,
                description = sale.description
            };
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByVehicleIdAsync(int vehicleId)
        {
            var sales = await _saleRepository.GetSalesByVehicleIdAsync(vehicleId);
            return sales.Select(sale => new SaleDto
            {
                id = sale.id,
                create_date = sale.create_date,
                availability_date = sale.availability_date,
                sale_date = sale.sale_date,
                price = sale.price,
                title = sale.title,
                description = sale.description
            }).ToList();
        }
    }
}