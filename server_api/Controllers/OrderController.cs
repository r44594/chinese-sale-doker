using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Chinese_sale_Api.Interfaces;

namespace Chinese_sale_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService service, ILogger<OrderController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get number of tickets for a gift
        /// </summary>
        [HttpGet("GetNumberOfTickets/{giftId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetNumberOfTickets(int giftId)
        {
            try
            {
                _logger.LogInformation("Fetching number of tickets for gift {GiftId}", giftId);
                var sum = await _service.GetNumberOfTickets(giftId);
                return Ok(sum);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching ticket count");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Order by total price (highest first)
        /// </summary>
        [HttpGet("MaxPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OrderByTotalPrice()
        {
            try
            {
                _logger.LogInformation("Fetching orders sorted by total price");
                var order = await _service.OrderByTotalPrice();
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error ordering by total price");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get the most purchased gift
        /// </summary>
        [HttpGet("maxschumgift")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMostPurchasedGift()
        {
            try
            {
                _logger.LogInformation("Fetching most purchased gift");
                var gift = await _service.GetMostPurchasedGift();

                if (gift == null)
                    return NotFound("No purchases found");

                return Ok(gift);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching most purchased gift");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get buyer details
        /// </summary>
        [HttpGet("GetBuyerDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBuyerDetails()
        {
            try
            {
                _logger.LogInformation("Fetching buyer details");
                var buyers = await _service.GetBuyerDetails();
                return Ok(buyers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching buyer details");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
