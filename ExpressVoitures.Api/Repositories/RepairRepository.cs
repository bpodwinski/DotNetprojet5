using ExpressVoituresApi.Data;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Repositories.Interfaces;

namespace ExpressVoituresApi.Repositories
{
    public class RepairRepository : IRepairRepository
    {
        private readonly ApplicationDbContext _context;

        public RepairRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Repair repair)
        {
            await _context.Repairs.AddAsync(repair);
            await _context.SaveChangesAsync();
        }
    }
}