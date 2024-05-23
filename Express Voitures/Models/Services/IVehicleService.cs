using System.Collections.Generic;
using System.Threading.Tasks;
using Express_Voitures.Dtos;
using Express_Voitures.Models.Entities;

namespace Express_Voitures.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(int id);
        Task<VehicleDto> GetVehicleWithPurchaseByIdAsync(int id);
        Task AddVehicleAsync(Vehicle vehicle);
        Task<bool> UpdateVehicleAsync(int id, Vehicle vehicle);
        Task DeleteVehicleAsync(int id);
    }
}