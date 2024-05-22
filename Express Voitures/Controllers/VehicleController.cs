using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Express_Voitures.Models.Entities;
using Express_Voitures.Services;

namespace Express_Voitures.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;

        public VehicleController(ILogger<VehicleController> logger, IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        // GET: /Vehicle
        [HttpGet(Name = "GetVehicles")]
        public async Task<IEnumerable<Vehicle>> Get()
        {
            return await _vehicleService.GetAllVehiclesAsync();
        }

        // GET: /Vehicle/{id}
        [HttpGet("{id}", Name = "GetVehicleById")]
        public async Task<ActionResult<Vehicle>> Get(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return vehicle;
        }

        // POST: /Vehicle
        [HttpPost(Name = "AddVehicle")]
        public async Task<ActionResult> Post([FromBody] Vehicle vehicle)
        {
            await _vehicleService.AddVehicleAsync(vehicle);
            return CreatedAtRoute("GetVehicleById", new { id = vehicle.Id }, vehicle);
        }

        // PUT: /Vehicle/{id}
        [HttpPut("{id}", Name = "UpdateVehicle")]
        public async Task<IActionResult> Put(int id, [FromBody] Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest("Vehicle data is missing");
            }

            var updated = await _vehicleService.UpdateVehicleAsync(id, vehicle);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: /Vehicle/{id}
        [HttpDelete("{id}", Name = "DeleteVehicle")]
        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleService.DeleteVehicleAsync(id);
            return NoContent();
        }
    }
}
