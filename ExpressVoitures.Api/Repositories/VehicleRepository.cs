using Microsoft.EntityFrameworkCore;
using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services;

namespace ExpressVoituresApi.Repositories
{
    /// <summary>
    /// Repository for managing vehicle entities.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public VehicleRepository(
            ApplicationDbContext context,
            ILogger<VehicleRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all vehicles.
        /// </summary>
        /// <returns>An IQueryable of vehicles.</returns>
        public IQueryable<Vehicle> GetAll()
        {
            try
            {
                return _context.Vehicles.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving vehicles");
                throw new InvalidOperationException($"An error occurred while retrieving vehicles", ex);
            }
        }

        /// <summary>
        /// Retrieves a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with the specified ID, or null if not found.</returns>
        public async Task<Vehicle?> GetById(int id)
        {
            try
            {
                var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.id == id);
                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving vehicle with ID {id}");
                throw new InvalidOperationException($"An error occurred while retrieving vehicle with ID {id}", ex);
            }
        }

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Add(Vehicle vehicle)
        {
            try
            {
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new vehicle");
                throw new InvalidOperationException("An error occurred while adding a new vehicle", ex);
            }
        }

        public async Task AddBulk(IEnumerable<Vehicle> vehicles)
        {
            await _context.Vehicles.AddRangeAsync(vehicles);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to update.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public async Task<bool> Update(Vehicle vehicle)
        {
            try
            {
                _context.Vehicles.Attach(vehicle);
                _context.Entry(vehicle).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uptading the vehicle");
                throw new InvalidOperationException("An error occurred while uptading the vehicle", ex);
            }
        }

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Delete(int id)
        {
            try
            {
                var vehicle = await _context.Vehicles.FindAsync(id);

                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {id} not found");
                }

                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the vehicle");
                throw new InvalidOperationException("An error occurred while deleting the vehicle", ex);
            }
        }

        /// <summary>
        /// Checks if a vehicle with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID of the vehicle to check.</param>
        /// <returns>True if the vehicle exists, false otherwise.</returns>
        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.id == id);
        }
    }
}
