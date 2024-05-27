using Microsoft.EntityFrameworkCore;
using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;

namespace ExpressVoituresApi.Repositories
{
    /// <summary>
    /// Repository for managing vehicle entities.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all vehicles.
        /// </summary>
        /// <returns>An IQueryable of vehicles.</returns>
        public IQueryable<Vehicle> GetAll()
        {
            return _context.Vehicles.AsQueryable();
        }

        /// <summary>
        /// Retrieves a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with the specified ID, or null if not found.</returns>
        public async Task<Vehicle?> GetById(int id)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.id == id);
        }

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle entity to update.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public async Task<bool> Update(Vehicle vehicle)
        {
            _context.Vehicles.Attach(vehicle);
            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(vehicle.id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
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
