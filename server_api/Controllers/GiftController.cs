using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using server_api.Dtos;
using server_api.Interfaces;
using server_api.Models;

namespace server_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftServices _services;
        private readonly ILogger<GiftController> _logger;

        public GiftController(IGiftServices services, ILogger<GiftController> logger)
        {
            _services = services;
            _logger = logger;
        }

        /// <summary>
        /// Get all gifts
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGifts()
        {
            try
            {
                _logger.LogInformation("Fetching all gifts");
                var gifts = await _services.GetAllGifts();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gifts");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get gift by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGiftById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching gift by id {Id}", id);
                var gift = await _services.GetGiftById(id);
                return Ok(gift);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid gift ID");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gift by ID");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Get donor details by gift ID
        /// </summary>
        [HttpGet("{id}/donor")]
        [ProducesResponseType(typeof(Donor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetDonorByGiftId(int id)
        {
            try
            {
                _logger.LogInformation("Fetching donor for gift ID: {GiftId}", id);
                var donor = await _services.GetDonorByGiftId(id);
                return Ok(donor);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching donor for gift ID: {GiftId}", id);

                if (ex.Message.Contains("לא קיים"))
                    return NotFound(ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Add new gift
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddGift([FromBody] GiftDto.CreatUpdate dto)
        {
            try
            {
                _logger.LogInformation("Adding gift: {@Gift}", dto);
                var newGift = await _services.AddGift(dto);
                return CreatedAtAction(nameof(GetGiftById), new { id = newGift.Id }, newGift);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid gift data");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding gift");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update existing gift
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateGift(int id, [FromBody] GiftDto.CreatUpdate dto)
        {
            _logger.LogInformation("Updating gift {Id} with {@Gift}", id, dto);
            try
            {
                var updatedGift = await _services.UpdateGift(id, dto);
                return Ok(updatedGift);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid gift update");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating gift");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete gift by ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteGift(int id)
        {
            try
            {
                _logger.LogInformation("Deleting gift {Id}", id);
                await _services.DeleteGift(id);
                return Ok("המתנה נמחקה בהצלחה");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting gift");
                if (ex.Message.Contains("לא ניתן למחוק מתנה"))
                    return Conflict(ex.Message);
                if (ex.Message.Contains("מתנה לא נמצאה"))
                    return NotFound(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Search gifts by gift name
        /// </summary>
        [HttpGet("search-by-name")]
        [ProducesResponseType(typeof(List<GiftDto.Get>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchGiftsByName([FromQuery] string giftName)
        {
            try
            {
                _logger.LogInformation("Searching gifts by name: {GiftName}", giftName);
                var gifts = await _services.SearchGiftsByName(giftName);
                return Ok(gifts);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid gift name search");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching gifts by name");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Search gifts by donor name
        /// </summary>
        [HttpGet("search-by-donor")]
        [ProducesResponseType(typeof(List<GiftDto.Get>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchByDonor([FromQuery] string donorName)
        {
            try
            {
                _logger.LogInformation("Searching gifts by donor name: {DonorName}", donorName);
                var gifts = await _services.SearchGiftsByDonorNameAsync(donorName);
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching gifts by donor");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get gifts by unique buyers count
        /// </summary>
        [HttpGet("by-buyers-count")]
        [ProducesResponseType(typeof(List<GiftDto.Get>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByBuyersCount([FromQuery] int buyerCount)
        {
            try
            {
                _logger.LogInformation("Fetching gifts by buyers count: {BuyerCount}", buyerCount);
                var gifts = await _services.GetGiftsByUniqueBuyerCountAsync(buyerCount);
                return Ok(gifts);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid buyers count");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gifts by buyers count");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get gifts by category name
        /// </summary>
        [HttpGet("by-category")]
        [ProducesResponseType(typeof(List<GiftDto.Get>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCategory([FromQuery] string categoryName)
        {
            try
            {
                _logger.LogInformation("Fetching gifts by category: {CategoryName}", categoryName);
                var gifts = await _services.GetGiftsByCategoryNameAsync(categoryName);
                return Ok(gifts);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid category name");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gifts by category");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get gifts ordered by price ascending
        /// </summary>
        [HttpGet("ordered-by-price")]
        [ProducesResponseType(typeof(List<GiftDto.Get>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderedByPrice()
        {
            try
            {
                _logger.LogInformation("Fetching gifts ordered by price ascending");
                var gifts = await _services.GetGiftsOrderedByPriceAscAsync();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gifts ordered by price");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get all gifts after lottery
        /// </summary>
        [HttpGet("after-random")]
        [ProducesResponseType(typeof(List<GiftDto.AfterRandom>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAfterRandom()
        {
            try
            {
                _logger.LogInformation("Fetching gifts after lottery");
                var gifts = await _services.GetAllGiftsAfterRandom();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gifts after lottery");
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet("get-all-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all categories");
                var categories = await _services.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
