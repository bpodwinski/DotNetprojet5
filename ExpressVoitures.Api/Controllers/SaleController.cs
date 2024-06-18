using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpressVoituresApi.Controllers
{
    [ApiController]
    [Route("vehicle/{id}/sale")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ISaleService saleService, ILogger<SaleController> logger)
        {
            _saleService = saleService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new sale to a vehicle
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
        /// <param name="saleAddDto">The sale data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost(Name = "AddSale")]
        public async Task<ActionResult> AddSale(int id, [FromBody] SaleAddDto saleAddDto)
        {
            try
            {
                if (saleAddDto == null)
                {
                    _logger.LogWarning("SaleDto is null");
                    return BadRequest(new { Message = "Sale data is required" });
                }

                saleAddDto.vehicle_id = id;
                await _saleService.AddSale(saleAddDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the sale");
                return StatusCode(500, new { Message = "An error occurred while adding the sale" });
            }
        }
    }
}
