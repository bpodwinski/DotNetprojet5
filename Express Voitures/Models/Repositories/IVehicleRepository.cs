using System.Collections.Generic;
using System.Threading.Tasks;
using Express_Voitures.Models.Entities;

namespace Express_Voitures.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task AddAsync(Vehicle vehicle);
        Task<bool> UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(int id);
    }
}