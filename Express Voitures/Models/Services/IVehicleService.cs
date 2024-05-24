using System.Collections.Generic;
using System.Threading.Tasks;
using Express_Voitures.Dtos;
using Express_Voitures.DTOs;
using Express_Voitures.Models.Entities;

namespace Express_Voitures.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int pageNumber, int pageSize, string brand, string sortOrder);
        Task<VehicleDto> GetVehicleByIdAsync(int id);
        Task<VehicleWithPurchaseDto> GetVehicleWithPurchaseByIdAsync(int id);
        Task AddPurchaseToVehicleAsync(int vehicleId, PurchaseDto purchaseDto);
        Task AddVehicleAsync(VehicleDto vehicleDto);
        Task<bool> UpdateVehicleAsync(int id, Vehicle vehicle);
        Task DeleteVehicleAsync(int id);
    }
}