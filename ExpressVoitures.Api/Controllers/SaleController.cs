using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Models.Entities;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoituresApi.Controllers
{
    [ApiController]
    [Route("vehicle/{id}/sale")]
    public class SaleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<SaleController> _logger;

        public SaleController(IVehicleService vehicleService, ILogger<SaleController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all sales for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>A list of sales associated with the vehicle.</returns>
        [HttpGet(Name = "GetSalesByVehicleId")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetSalesByVehicleId(int vehicleId)
        {
            try
            {
                var sales = await _vehicleService.GetSalesByVehicleIdAsync(vehicleId);
                return Ok(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving sales for vehicle ID {vehicleId}");
                return StatusCode(500, new { Message = "An error occurred while retrieving the sales" });
            }
        }

        /// <summary>
        /// Retrieves a specific sale by ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="saleId">The ID of the sale.</param>
        /// <returns>The sale with the specified ID.</returns>
        [HttpGet("{saleId}", Name = "GetSaleById")]
        public async Task<ActionResult<SaleDto>> GetSaleById(int vehicleId, int saleId)
        {
            try
            {
                var sale = await _vehicleService.GetSaleByIdAsync(saleId);
                if (sale == null || sale.vehicle_id != vehicleId)
                {
                    _logger.LogWarning($"Sale with ID {saleId} not found for vehicle ID {vehicleId}");
                    return NotFound(new { Message = $"Sale with ID {saleId} not found for vehicle ID {vehicleId}" });
                }
                return Ok(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving sale with ID {saleId}");
                return StatusCode(500, new { Message = "An error occurred while retrieving the sale" });
            }
        }

        /// <summary>
        /// Adds a new sale to a vehicle
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
        /// <param name="saleAddDto">The sale data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost(Name = "AddSaleToVehicle")]
        public async Task<ActionResult> AddSaleToVehicle(int id, [FromBody] SaleAddDto saleAddDto)
        {
            try
            {
                if (saleAddDto == null)
                {
                    _logger.LogWarning("SaleDto is null");
                    return BadRequest(new { Message = "Sale data is required" });
                }

                saleAddDto.vehicle_id = id;
                await _vehicleService.AddSaleAsync(saleAddDto);
                return CreatedAtRoute("GetSaleById", new { vehicleId = id, saleId = saleAddDto.id }, saleAddDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the sale");
                return StatusCode(500, new { Message = "An error occurred while adding the sale" });
            }
        }

        /// <summary>
        /// Deletes a sale by ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <param name="saleId">The ID of the sale.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete(Name = "DeleteSale")]
        public async Task<IActionResult> DeleteSale(int vehicleId, int saleId)
        {
            try
            {
                var sale = await _vehicleService.GetSaleByIdAsync(saleId);
                if (sale == null || sale.vehicle_id != vehicleId)
                {
                    _logger.LogWarning($"Sale with ID {saleId} not found for vehicle ID {vehicleId}");
                    return NotFound(new { Message = $"Sale with ID {saleId} not found for vehicle ID {vehicleId}" });
                }

                await _vehicleService.DeleteSaleAsync(saleId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting sale with ID {saleId}");
                return StatusCode(500, new { Message = "An error occurred while deleting the sale" });
            }
        }
    }
}
