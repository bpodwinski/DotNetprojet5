using ExpressVoituresApi.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    public interface ISaleRepository
    {
        Task AddAsync(Sale sale);
        Task<Sale> GetByIdAsync(int saleId);
        Task<List<Sale>> GetSalesByVehicleIdAsync(int vehicleId);
        Task DeleteAsync(int saleId);
    }
}