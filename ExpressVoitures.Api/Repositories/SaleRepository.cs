using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressVoituresApi.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        public async Task<Sale> GetByIdAsync(int saleId)
        {
            return await _context.Sales.FindAsync(saleId);
        }

        public async Task<List<Sale>> GetSalesByVehicleIdAsync(int vehicleId)
        {
            return await _context.Sales
                .Where(s => s.vehicle_id == vehicleId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int saleId)
        {
            var sale = await GetByIdAsync(saleId);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }
    }
}