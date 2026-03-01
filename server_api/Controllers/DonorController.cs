using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_api.Dtos;
using server_api.Interfaces;
using server_api.Models;

namespace server_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorServices _donorServices;
        private readonly ILogger<DonorController> _logger;

        public DonorController(IDonorServices donorServices, ILogger<DonorController> logger)
        {
            _donorServices = donorServices;
            _logger = logger;
        }

        /// <summary>
        /// Add new donor
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDonor([FromBody] DonorDto.CreatUpdateDonor dto)
        {
            try
            {
                _logger.LogInformation("Adding donor {@Donor}", dto);
                await _donorServices.AddDonor(dto);
                return Ok("התורם נוסף בהצלחה");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid donor data");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding donor");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get all donors
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<GetDonor>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDonors()
        {
            try
            {
                _logger.LogInformation("Fetching all donors");
                var donors = await _donorServices.GetAllDonors();
                return Ok(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching donors");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get donor by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(GetDonor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDonorById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching donor by id {Id}", id);
                var donor = await _donorServices.GetDonorById(id);
                return Ok(donor);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Donor not found");
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get donor by full name
        /// </summary>
        /// 

        [HttpGet("by-name")]
        [ProducesResponseType(typeof(GetDonor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByDonorName([FromQuery] string donorName)
        {
            try
            {
                _logger.LogInformation("Fetching donor by name {Name}", donorName);
                var donor = await _donorServices.GetByDonorName(donorName);
                return Ok(donor);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Donor not found by name");
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get donors by email
        /// </summary>
        [HttpGet("by-email")]
        [ProducesResponseType(typeof(List<GetDonor>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDonorEmail([FromQuery] string email)
        {
            try
            {
                _logger.LogInformation("Fetching donors by email {Email}", email);
                var donors = await _donorServices.GetByDonorEmail(email);
                return Ok(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching donors by email");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get donors by gift name
        /// </summary>
        [HttpGet("by-gift")]
        [ProducesResponseType(typeof(List<GetDonor>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDonorGift([FromQuery] string giftName)
        {
            try
            {
                _logger.LogInformation("Fetching donors by gift name {GiftName}", giftName);
                var donors = await _donorServices.GetByDonorGift(giftName);
                return Ok(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching donors by gift");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update donor
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Donor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDonor(int id, [FromBody] DonorDto.CreatUpdateDonor dto)
        {
            try
            {
                _logger.LogInformation("Updating donor {Id}", id);
                var donor = await _donorServices.UpdateDonor(id, dto);
                return Ok(donor);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error updating donor");
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Delete donor by ID
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            try
            {
                _logger.LogInformation("Deleting donor with ID {Id}", id);
                await _donorServices.DeleteDonor(id);
                return Ok("התורם נמחק בהצלחה");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting donor");
                return NotFound(ex.Message);
            }
        }

    }
}
