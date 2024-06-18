using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services.Interfaces;

namespace ExpressVoituresApi.Services
{
    /// <summary>
    /// Service for managing vehicles.
    /// </summary>
    public class SaleService : ISaleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<SaleService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleService"/> class.
        /// </summary>
        /// <param name="saleRepository">The sale repository.</param>
        /// <param name="logger">The logger.</param>
        public SaleService(
            IVehicleRepository vehicleRepository,
            ISaleRepository saleRepository,
            ILogger<SaleService> logger
            )
        {
            _vehicleRepository = vehicleRepository;
            _saleRepository = saleRepository;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new sale to a vehicle.
        /// </summary>
        /// <param name="saleAddDto">The sale data transfer object containing the details of the sale.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the vehicle with the specified ID is not found,
        /// when the vehicle already has a sale,
        /// or when an error occurs while adding the sale.
        /// </exception>
        public async Task AddSale(SaleAddDto saleAddDto)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetById(saleAddDto.vehicle_id);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {saleAddDto.vehicle_id} not found");
                }

                if (vehicle.sale != null)
                {
                    throw new InvalidOperationException("The vehicle already has a sale");
                }

                var sale = new Brand
                {
                    vehicle_id = saleAddDto.vehicle_id,
                    create_date = saleAddDto.create_date,
                    availability_date = saleAddDto.availability_date,
                    sale_date = saleAddDto.sale_date,
                    price = saleAddDto.price,
                    title = saleAddDto.title,
                    description = saleAddDto.description
                };

                await _saleRepository.Add(sale);
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
        public async Task DeleteSale(int saleId)
        {
            await _saleRepository.Delete(saleId);
        }
    }
}