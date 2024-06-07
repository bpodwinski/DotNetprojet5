using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;

namespace ExpressVoituresApi.Repositories
{
    /// <summary>
    /// Repository for managing repair entities.
    /// </summary>
    public class RepairRepository : IRepairRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepairRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public RepairRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new repair.
        /// </summary>
        /// <param name="repair">The repair entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Add(Repair repair)
        {
            await _context.Repairs.AddAsync(repair);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing repair.
        /// </summary>
        /// <param name="repair">The repair entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Update(Repair repair)
        {
            _context.Repairs.Update(repair);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a repair by its ID.
        /// </summary>
        /// <param name="id">The ID of the repair to retrieve.</param>
        /// <returns>The repair entity or null if not found.</returns>
        public async Task<Repair?> GetById(int id)
        {
            return await _context.Repairs.FindAsync(id);
        }

        /// <summary>
        /// Deletes a repair by ID.
        /// </summary>
        /// <param name="id">The ID of the repair to delete.</param>
        /// <returns>True if the repair was deleted, false if not found.</returns>
        public async Task<bool> Delete(int id)
        {
            var repair = await GetById(id);
            if (repair == null)
            {
                return false;
            }

            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
