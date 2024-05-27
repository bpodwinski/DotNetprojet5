using Microsoft.AspNetCore.Mvc;
using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Services.Interfaces;

namespace ExpressVoituresApi.Controllers
{
    [ApiController]
    [Route("vehicle/{id}/purchase")]
    public class PurchaseController : ControllerBase
    {
        private readonly ILogger<PurchaseController> _logger;
        private readonly IVehicleService _vehicleService;

        public PurchaseController(
            ILogger<PurchaseController> logger,
            IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Adds a new purchase to a vehicle
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
        /// <param name="purchaseDto">The purchase data transfer object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="200">Purchase added successfully.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        /// <response code="404">If the vehicle is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        // POST: /vehicle/{id}/purchase
        [HttpPost(Name = "AddPurchase")]
        public async Task<ActionResult> AddPurchase(int id, [FromBody] PurchaseDto purchaseDto)
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

                await _vehicleService.AddPurchase(id, purchaseDto);
                return Ok();
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
        /// Deletes the purchase associated with a vehicle
        /// </summary>
        /// <param name="id">The ID of the vehicle whose purchase will be deleted.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        /// <response code="204">Purchase deleted successfully.</response>
        /// <response code="404">If the vehicle or purchase is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        // DELETE: /vehicle/{id}/purchase
        [HttpDelete(Name = "DeletePurchase")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            try
            {
                await _vehicleService.DeletePurchase(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting purchase with ID {id}");
                return StatusCode(500, new { Message = "An error occurred while deleting the purchase" });
            }
        }
    }
}
