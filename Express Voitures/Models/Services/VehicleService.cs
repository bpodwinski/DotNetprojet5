using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Express_Voitures.Dtos;
using Express_Voitures.DTOs;
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

        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Select(vehicle => new VehicleDto
            {
                Id = vehicle.Id,
                CreateDate = vehicle.CreateDate,
                Vin = vehicle.Vin,
                Year = vehicle.Year,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                TrimLevel = vehicle.TrimLevel
            }).ToList();
        }

        public async Task<VehicleDto> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                return null;
            }

            return new VehicleDto
            {
                Id = vehicle.Id,
                CreateDate = vehicle.CreateDate,
                Vin = vehicle.Vin,
                Year = vehicle.Year,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                TrimLevel = vehicle.TrimLevel
            };
        }

        public async Task<VehicleWithPurchaseDto> GetVehicleWithPurchaseByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdWithPurchaseAsync(id);
            if (vehicle == null)
            {
                return null;
            }

            return new VehicleWithPurchaseDto
            {
                Id = vehicle.Id,
                CreateDate = vehicle.CreateDate,
                Vin = vehicle.Vin,
                Year = vehicle.Year,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                TrimLevel = vehicle.TrimLevel,
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