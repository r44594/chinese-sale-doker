using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Chinese_sale_Api.Interfaces;
using server_api.Dtos;
using server_api.Models;

namespace Chinese_sale_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _service;
        private readonly ILogger<BasketController> _logger;

        public BasketController(
            IBasketService service,
            ILogger<BasketController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Add item to user basket (saved as draft)
        /// </summary>
        [HttpPost("add-to-basket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddToBasket([FromBody] BasketDto basketDto)
        {
            _logger.LogInformation("AddToBasket called with data: {@basketDto}", basketDto);

            try
            {
                await _service.AddToBasket(basketDto);

                _logger.LogInformation("Item successfully added to basket");

                return Ok(new { message = "הפריט נוסף לסל בהצלחה" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while adding item to basket");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding item to basket");
                return StatusCode(500, new { message = "שגיאה בשרת" });
            }
        }

        /// <summary>
        /// Checkout basket and create order
        /// </summary>
        [HttpPost("checkout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto checkoutDto)
        {
            _logger.LogInformation("Checkout started for userId {userId}", checkoutDto.UserId);

            try
            {
                var orderId = await _service.Checkout(checkoutDto);

                _logger.LogInformation("Checkout completed successfully. OrderId: {orderId}", orderId);

                return Ok(new { orderId });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Checkout failed for userId {userId}", checkoutDto.UserId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during checkout for userId {userId}", checkoutDto.UserId);
                return StatusCode(500, new { message = "שגיאה בשרת" });
            }
        }

        //תכתוב לי פוןנקציה זו-  public async Task<Basket?> GetBasketByUserIdAsync(int userId)-עם טריי וקאטש מהסגנון של הפונקציות למעלה
        /// <summary>
        ///
        // BasketController.cs

        [HttpGet("{userId}")] // זה יגרום לנתיב להיות api/Basket/1011
        public async Task<IActionResult> GetBasketByUserIdAsync(int userId)
        {
            _logger.LogInformation("GetBasketByUserIdAsync called for userId {userId}", userId);
            try
            {
                var basket = await _service.GetBasketByUserIdAsync(userId);

                if (basket == null)
                {
                    _logger.LogInformation("No basket found for userId {userId}, returning empty object", userId);
                    // חשוב: השמות כאן צריכים להתאים בדיוק למה שה-Angular מצפה (camelCase)
                    return Ok(new { userId = userId, basketItems = new List<object>() });
                }

                _logger.LogInformation("Basket retrieved successfully for userId {userId}", userId);
                return Ok(basket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching basket for userId {userId}", userId);
                return StatusCode(500, new { message = "שגיאה בשרת" });
            }
        }
        /// <summary>
        /// Remove a specific item from the user's basket
        /// </summary>
        /// 
        [HttpDelete("remove-item/{basketItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveItemFromBasket(int basketItemId)
        {
            _logger.LogInformation("RemoveItemFromBasket Controller called for ID: {basketItemId}", basketItemId);

            try
            {
                // קריאה לסרביס שמכיל את הלוגיקה
                await _service.RemoveBasketItemAsync(basketItemId);

                _logger.LogInformation("Item {basketItemId} successfully removed", basketItemId);
                return Ok(new { message = "הפריט הוסר מהסל בהצלחה" });
            }
            catch (ArgumentException ex)
            {
                // כאן נתפסות שגיאות לוגיות (כמו פריט שלא נמצא)
                _logger.LogWarning(ex, "Validation error in RemoveItemFromBasket for ID {basketItemId}", basketItemId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // כאן נתפסות שגיאות שרת לא צפויות
                _logger.LogError(ex, "Unexpected error in RemoveItemFromBasket for ID {basketItemId}", basketItemId);
                return StatusCode(500, new { message = "שגיאה פנימית בשרת בעת הסרת הפריט" });
            }
        }


    }
}
