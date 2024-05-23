using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Express_Voitures.Dtos;
using Express_Voitures.Models.Entities;
using Express_Voitures.Repositories;

namespace Express_Voitures.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        public async Task<VehicleDto> GetVehicleWithPurchaseByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdWithPurchaseAsync(id);
            if (vehicle == null)
            {
                return null;
            }

            return new VehicleDto
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Purchase = vehicle.Purchase != null ? new PurchaseDto
                {
                    Id = vehicle.Purchase.Id,
                    Date = vehicle.Purchase.Date,
                    Price = vehicle.Purchase.Price,
                } : null
            };
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task<bool> UpdateVehicleAsync(int id, Vehicle vehicle)
        {
            vehicle.Id = id;
            return await _vehicleRepository.UpdateAsync(vehicle);
        }


        public async Task DeleteVehicleAsync(int id)
        {
            await _vehicleRepository.DeleteAsync(id);
        }
    }
}