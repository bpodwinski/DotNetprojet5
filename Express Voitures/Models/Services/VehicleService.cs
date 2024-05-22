using System.Collections.Generic;
using System.Threading.Tasks;
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