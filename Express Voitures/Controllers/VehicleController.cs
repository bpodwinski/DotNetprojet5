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

        /// <summary>
        /// Retrieves a list of vehicles.
        /// </summary>
        /// <returns>A list of vehicles.</returns>
        /// <response code="200">Returns the list of vehicles.</response>
        /// <response code="500">If there is an internal server error.</response>
        // GET: /Vehicle
        [HttpGet(Name = "GetVehicles")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Get()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        /// <summary>
        /// Retrieves a vehicle by ID.
        /// </summary>
        /// <returns>A list of vehicles.</returns>
        /// <response code="200">Returns the list of vehicles.</response>
        /// <response code="500">If there is an internal server error.</response>
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

        /// <summary>
        /// Retrieves a vehicle by ID with its purchase information.
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
        /// <returns>The vehicle with its purchase information.</returns>
        /// <response code="200">Returns the vehicle with purchase information.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        // GET: /Vehicle/{id}/Purchase
        [HttpGet("{id}/Purchase", Name = "GetVehicleWithPurchaseById")]
        public async Task<ActionResult<VehicleWithPurchaseDto>> GetVehicleWithPurchaseById(int id)
        {
            var vehicle = await _vehicleService.GetVehicleWithPurchaseByIdAsync(id);
            if (vehicle == null)
            {
                _logger.LogWarning($"Vehicle with ID {id} not found");
                return NotFound(new { Message = $"Vehicle with ID {id} not found" });
            }
            return Ok(vehicle);
        }

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicleDto">The vehicle data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="201">Vehicle created successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        // POST: /Vehicle
        [HttpPost(Name = "AddVehicle")]
        public async Task<ActionResult> Post([FromBody] VehicleDto vehicleDto)
        {
            try
            {
                await _vehicleService.AddVehicleAsync(vehicleDto);
                return CreatedAtRoute("GetVehicleById", new { id = vehicleDto.Id }, vehicleDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the vehicle.");
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Adds a new purchase to a vehicle.
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="200">Purchase added successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        // POST: /Vehicle/{id}/Purchase
        [HttpPost("{id}/Purchase", Name = "AddPurchaseToVehicle")]
        public async Task<ActionResult> AddPurchaseToVehicle(int id, [FromBody] PurchaseDto purchaseDto)
        {
            try
            {
                await _vehicleService.AddPurchaseToVehicleAsync(id, purchaseDto);
                return Ok(new { Message = "Purchase added successfully" });
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
                _logger.LogError(ex, "An error occurred while adding the purchase");
                return StatusCode(500, new { Message = "An error occurred while adding the purchase" });
            }
        }

        /// <summary>
        /// Updates a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to update.</param>
        /// <param name="vehicle">The vehicle data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="204">Vehicle updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
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

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="204">Vehicle deleted successfully.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
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
