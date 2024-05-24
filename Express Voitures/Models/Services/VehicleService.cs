using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Express_Voitures.Dtos;
using Express_Voitures.DTOs;
using Express_Voitures.Models.Entities;
using Express_Voitures.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Express_Voitures.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public VehicleService(
            IVehicleRepository vehicleRepository,
            IPurchaseRepository purchaseRepository
            )
        {
            _vehicleRepository = vehicleRepository;
            _purchaseRepository = purchaseRepository;
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

        public async Task AddVehicleAsync(VehicleDto vehicleDto)
        {
            try
            {
                var vehicle = new Vehicle
                {
                    Id = vehicleDto.Id,
                    CreateDate = vehicleDto.CreateDate,
                    Vin = vehicleDto.Vin,
                    Year = vehicleDto.Year,
                    Brand = vehicleDto.Brand,
                    Model = vehicleDto.Model,
                    TrimLevel = vehicleDto.TrimLevel
                };

                await _vehicleRepository.AddAsync(vehicle);
            }
            catch (DbUpdateException ex)
            {
                // Log the detailed error and throw a custom exception or handle it as needed
                throw new InvalidOperationException("An error occurred while adding the vehicle to the database.", ex);
            }
            catch (Exception ex)
            {
                // Log the detailed error and throw a custom exception or handle it as needed
                throw new InvalidOperationException("An unexpected error occurred while adding the vehicle.", ex);
            }
        }

        public async Task AddPurchaseToVehicleAsync(int vehicleId, PurchaseDto purchaseDto)
        {
            var vehicle = await _vehicleRepository.GetByIdWithPurchaseAsync(vehicleId);
            if (vehicle == null)
            {
                throw new ArgumentException($"Vehicle with ID {vehicleId} not found");
            }

            if (vehicle.Purchase != null)
            {
                throw new InvalidOperationException("The vehicle already has a purchase");
            }

            var purchase = new Purchase
            {
                Date = purchaseDto.Date,
                Price = purchaseDto.Price,
                VehicleId = vehicleId
            };

            await _purchaseRepository.AddAsync(purchase);
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