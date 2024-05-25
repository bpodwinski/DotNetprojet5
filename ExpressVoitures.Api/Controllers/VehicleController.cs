using Microsoft.AspNetCore.Mvc;
using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Services;

namespace ExpressVoituresApi.Controllers
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
        /// Retrieves a list of vehicles with optional pagination, filtering, and sorting.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination (default is 1).</param>
        /// <param name="pageSize">The page size for pagination (default is 25).</param>
        /// <param name="filter">Optional filter criteria.</param>
        /// <param name="sortOrder">Optional sort order ("id", "year", "brand", "model").</param>
        /// <returns>A list of vehicles.</returns>
        /// <response code="200">Returns the list of vehicles.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        // GET: /Vehicle
        [HttpGet(Name = "GetVehicles")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string brand = null,
            [FromQuery] string sortOrder = null)
        {
            try
            {
                if (pageNumber < 1)
                {
                    return BadRequest(new { Message = "Page number must be greater than or equal to 1" });
                }

                if (pageSize < 1)
                {
                    return BadRequest(new { Message = "Page size must be greater than or equal to 1" });
                }

                var vehicles = await _vehicleService.GetVehiclesAsync(pageNumber, pageSize, brand, sortOrder);
                return Ok(vehicles);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the vehicles");
                return StatusCode(500, new { Message = "An error occurred while retrieving the vehicles" });
            }
        }

        /// <summary>
        /// Retrieves a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with the specified ID.</returns>
        /// <response code="200">Returns the vehicle with the specified ID.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        // GET: /Vehicle/{id}
        [HttpGet("{id}", Name = "GetVehicleById")]
        public async Task<ActionResult<VehicleDto>> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid ID: {id}");
                    return BadRequest(new { Message = "ID must be greater than 0" });
                }

                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    _logger.LogWarning($"Vehicle with ID {id} not found");
                    return NotFound(new { Message = $"Vehicle with ID {id} not found" });
                }
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving vehicle with ID {id}");
                return StatusCode(500, new { Message = "An error occurred while retrieving the vehicle" });
            }
        }

        /// <summary>
        /// Retrieves a vehicle by ID with its purchase information.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>The vehicle with its purchase information.</returns>
        /// <response code="200">Returns the vehicle with purchase information.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("{id}/Purchase", Name = "GetVehicleWithPurchaseById")]
        public async Task<ActionResult<VehicleWithPurchaseDto>> GetVehicleWithPurchaseById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid ID: {id}");
                    return BadRequest(new { Message = "ID must be greater than 0" });
                }

                var vehicle = await _vehicleService.GetVehicleWithPurchaseByIdAsync(id);
                if (vehicle == null)
                {
                    _logger.LogWarning($"Vehicle with ID {id} not found");
                    return NotFound(new { Message = $"Vehicle with ID {id} not found" });
                }
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving vehicle with ID {id} and its purchase information");
                return StatusCode(500, new { Message = "An error occurred while retrieving the vehicle and its purchase information" });
            }
        }

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicleDto">The vehicle data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="201">Vehicle created successfully.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        // POST: /Vehicle
        [HttpPost(Name = "AddVehicle")]
        public async Task<ActionResult> Post([FromBody] VehicleDto vehicleDto)
        {
            try
            {
                if (vehicleDto == null)
                {
                    _logger.LogWarning("VehicleDto is null");
                    return BadRequest(new { Message = "Vehicle data is required" });
                }

                await _vehicleService.AddVehicleAsync(vehicleDto);
                return CreatedAtRoute("GetVehicleById", new { id = vehicleDto.Id }, vehicleDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the vehicle");
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the vehicle");
                return StatusCode(500, new { Message = "An unexpected error occurred while adding the vehicle" });
            }
        }

        /// <summary>
        /// Adds a new purchase to a vehicle.
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="200">Purchase added successfully.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        // POST: /Vehicle/{id}/Purchase
        [HttpPost("{id}/Purchase", Name = "AddPurchaseToVehicle")]
        public async Task<ActionResult> AddPurchaseToVehicle(int id, [FromBody] PurchaseDto purchaseDto)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid ID: {id}");
                    return BadRequest(new { Message = "ID must be greater than 0" });
                }

                if (purchaseDto == null)
                {
                    _logger.LogWarning("PurchaseDto is null");
                    return BadRequest(new { Message = "Purchase data is required" });
                }

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
        /// <param name="vehicleDto">The vehicle data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="204">Vehicle updated successfully.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        // PUT: /Vehicle/{id}
        [HttpPut("{id}", Name = "UpdateVehicle")]
        public async Task<IActionResult> Put(int id, [FromBody] VehicleDto vehicleDto)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid ID: {id}");
                    return BadRequest(new { Message = "ID must be greater than 0" });
                }

                if (vehicleDto == null)
                {
                    _logger.LogWarning("Vehicle data is null.");
                    return BadRequest(new { Message = "Vehicle data is required" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updated = await _vehicleService.UpdateVehicleAsync(id, vehicleDto);
                if (!updated)
                {
                    _logger.LogWarning($"Update failed. Vehicle with ID {id} not found");
                    return NotFound(new { Message = $"Vehicle with ID {id} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating vehicle with ID {id}");
                return StatusCode(500, new { Message = "An error occurred while updating the vehicle" });
            }
        }

        /// <summary>
        /// Deletes a vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="204">Vehicle deleted successfully.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        // DELETE: /Vehicle/{id}
        [HttpDelete("{id}", Name = "DeleteVehicle")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid ID: {id}");
                    return BadRequest(new { Message = "ID must be greater than 0" });
                }

                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    _logger.LogWarning($"Delete failed. Vehicle with ID {id} not found");
                    return NotFound(new { Message = $"Vehicle with ID {id} not found" });
                }

                await _vehicleService.DeleteVehicleAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting vehicle with ID {id}");
                return StatusCode(500, new { Message = "An error occurred while deleting the vehicle" });
            }
        }
    }
}
