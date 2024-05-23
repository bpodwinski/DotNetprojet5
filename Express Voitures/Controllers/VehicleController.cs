using Microsoft.AspNetCore.Mvc;
using Express_Voitures.Models.Entities;
using Express_Voitures.Services;
using Express_Voitures.Dtos;
using Express_Voitures.DTOs;

namespace Express_Voitures.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;
        public VehicleController(
            ILogger<VehicleController> logger,
            IVehicleService vehicleService
            )
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        // GET: /Vehicle
        [HttpGet(Name = "GetVehicles")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Get()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        // GET: /Vehicle/{id}
        [HttpGet("{id}", Name = "GetVehicleById")]
        public async Task<ActionResult<VehicleDto>> Get(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                _logger.LogWarning($"Vehicle with ID {id} not found.");
                return NotFound(new { Message = $"Vehicle with ID {id} not found." });
            }
            return Ok(vehicle);
        }

        // GET: /Vehicle/{id}/Purchase
        [HttpGet("{id}/Purchase", Name = "GetVehicleWithPurchaseById")]
        public async Task<ActionResult<VehicleWithPurchaseDto>> GetVehicleWithPurchaseById(int id)
        {
            var vehicle = await _vehicleService.GetVehicleWithPurchaseByIdAsync(id);
            if (vehicle == null)
            {
                _logger.LogWarning($"Vehicle with ID {id} not found.");
                return NotFound(new { Message = $"Vehicle with ID {id} not found." });
            }
            return Ok(vehicle);
        }

        // POST: /Vehicle
        [HttpPost(Name = "AddVehicle")]
        public async Task<ActionResult> Post([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _vehicleService.AddVehicleAsync(vehicle);
            return CreatedAtRoute("GetVehicleById", new { id = vehicle.Id }, vehicle);
        }

        // POST: /Vehicle/{id}/Purchase
        [HttpPost("{id}/Purchase", Name = "AddPurchaseToVehicle")]
        public async Task<ActionResult> AddPurchaseToVehicle(int id, [FromBody] PurchaseDto purchaseDto)
        {
            try
            {
                await _vehicleService.AddPurchaseToVehicleAsync(id, purchaseDto);
                return Ok(new { Message = "Purchase added successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the purchase.");
                return StatusCode(500, new { Message = "An error occurred while adding the purchase." });
            }
        }

        // PUT: /Vehicle/{id}
        [HttpPut("{id}", Name = "UpdateVehicle")]
        public async Task<IActionResult> Put(int id, [FromBody] Vehicle vehicle)
        {
            if (vehicle == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _vehicleService.UpdateVehicleAsync(id, vehicle);
            if (!updated)
            {
                _logger.LogWarning($"Update failed. Vehicle with ID {id} not found.");
                return NotFound(new { Message = $"Vehicle with ID {id} not found." });
            }

            return NoContent();
        }

        // DELETE: /Vehicle/{id}
        [HttpDelete("{id}", Name = "DeleteVehicle")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                _logger.LogWarning($"Delete failed. Vehicle with ID {id} not found.");
                return NotFound(new { Message = $"Vehicle with ID {id} not found." });
            }

            await _vehicleService.DeleteVehicleAsync(id);

            return NoContent();
        }
    }
}
