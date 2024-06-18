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
        private readonly IRepairService _repairService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepairController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="repairService">The repair service instance.</param>
        public RepairController(
            ILogger<RepairController> logger,
            IRepairService repairService)
        {
            _logger = logger;
            _repairService = repairService;
        }

        /// <summary>
        /// Adds a new repair to a vehicle.
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
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
                await _repairService.AddRepair(id, repairAddDto);
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
