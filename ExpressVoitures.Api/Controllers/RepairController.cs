using Microsoft.AspNetCore.Mvc;
using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Services.Interfaces;

namespace ExpressVoituresApi.Controllers
{
    [ApiController]
    [Route("vehicle/{id}/repair")]
    public class RepairController : ControllerBase
    {
        private readonly ILogger<RepairController> _logger;
        private readonly IVehicleService _vehicleService;

        public RepairController(
            ILogger<RepairController> logger,
            IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Adds a new repair to a vehicle
        /// </summary>
        /// <param name="repairAddDto">The repair data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost(Name = "AddRepair")]
        public async Task<ActionResult> AddRepair(int id, [FromBody] RepairAddDto repairAddDto)
        {
            try
            {
                if (repairAddDto == null)
                {
                    _logger.LogWarning("RepairDto is null");
                    return BadRequest(new { Message = "Repair data is required" });
                }

                repairAddDto.vehicle_id = id;
                await _vehicleService.AddRepair(id, repairAddDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the repair");
                return StatusCode(500, new { Message = "An error occurred while adding the repair" });
            }
        }
    }
}