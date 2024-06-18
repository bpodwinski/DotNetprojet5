using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressVoituresApi.Repositories
{
    /// <summary>
    /// Repository for managing sale entities.
    /// </summary>
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new sale.
        /// </summary>
        /// <param name="sale">The sale entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Add(Brand sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing sale.
        /// </summary>
        /// <param name="sale">The sale entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Update(Brand sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <returns>A task representing the asynchronous operation with a boolean result indicating success.</returns>
        public async Task<bool> Delete(int id)
        {
            var sale = await GetById(id);
            if (sale == null)
            {
                return false;
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to retrieve.</param>
        /// <returns>The sale entity or null if not found.</returns>
        public async Task<Brand?> GetById(int id)
        {
            return await _context.Sales.FindAsync(id);
        }
    }
}
