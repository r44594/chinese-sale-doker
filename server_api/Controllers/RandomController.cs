using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_api.Dtos;
using server_api.Interfaces;

namespace server_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RandomController : ControllerBase
    {
        private readonly IRandomService _service;
        private readonly ILogger<RandomController> _logger;

        public RandomController(IRandomService service, ILogger<RandomController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Draw lottery for all gifts
        /// </summary>
        [HttpGet("Random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DrawLottery()
        {
            try
            {
                _logger.LogInformation("Starting lottery draw");
                var winner = await _service.DrawLottery();
                return Ok(winner);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error during lottery draw");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get winners report
        /// </summary>
        [HttpGet("winner_of_gift")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWinnersReport()
        {
            try
            {
                _logger.LogInformation("Fetching winners report");
                var report = await _service.GetWinnersReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching winners report");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get total income from all gifts
        /// </summary>
        [HttpGet("zover_all_buy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalIncome()
        {
            try
            {
                _logger.LogInformation("Calculating total income");
                var income = await _service.GetTotalIncomeAsync();
                return Ok(income);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total income");
                return StatusCode(500, new { message = "שגיאה בשרת", details = ex.Message });
            }
        }
       
    }
}
