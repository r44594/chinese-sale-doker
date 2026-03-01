using Microsoft.AspNetCore.Mvc;
using server_api.Dtos;
using server_api.Interfaces;

namespace server_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserServices service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDto.RegisterDto dto)
        {
            try
            {
                _logger.LogInformation("Registering new user: {@User}", dto);
                var user = await _service.register(dto);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized registration attempt");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return StatusCode(500, new { message = "שגיאה בשרת", details = ex.Message });
            }
        }

        /// <summary>
        /// Login existing user
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                _logger.LogInformation("User login attempt: {@Login}", dto);
                var user = await _service.login(dto);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized login attempt");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "שגיאה בשרת", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
    //    [HttpGet]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //    public async Task<IActionResult> GetAllUsers()
    //    {
    //        try
    //        {
    //            _logger.LogInformation("Fetching all users");
    //            var users = await _service.GetAllUser();
    //            return Ok(users);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error fetching users");
    //            return StatusCode(500, new { message = ex.Message });
    //        }
    //    }
   }
}
