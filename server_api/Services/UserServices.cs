using Chinese_sale_Api.Dtos;
using server_api.Dtos;
using server_api.Interfaces;
using server_api.Models;

using StoreApi.Services;
using static server_api.Models.User;

namespace server_api.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUsereRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserServices> _logger;
        public UserServices(ITokenService tokenService, IUsereRepository repositories
            , IConfiguration configuration, ILogger<UserServices> logger)
        {
            _repository = repositories;
            _tokenService = tokenService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<GetUsers> register(UserDto.RegisterDto dto)
        {
            _logger.LogInformation("Registering new user with username: {UserName}", dto.UserName);
            var UserName = await _repository.GetByUserName(dto.UserName);
            if (UserName != null)
            {
                _logger.LogWarning("Username {UserName} already exists", dto.UserName); // לוג שגיאה
                throw new UnauthorizedAccessException(" שם זה כבר קיים במערכת!");
            }
            var user = new User
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                RoleUser = Role.User,
                Password = HashPassword(dto.Password),
           
            };

            var createdUser = await _repository.register(user);
            _logger.LogInformation("User created with ID: {UserId}", createdUser.Id);
            return MapToResponseDto(createdUser);

        }

        public async Task<LoginResponseDto?> login(LoginDto dto)
        {
            _logger.LogInformation("Login attempt for username/email: {UserName}", dto.UserName); // לוג מינימלי
            var user = await _repository.login(dto);
            if (user == null)
            {
                _logger.LogWarning("Login failed - user not found for {UserName}", dto.UserName); // לוג שגיאה
                
                return null;
            }
            var hashedPassword = HashPassword(dto.Password);
            if (user.Password != hashedPassword)
            {
                _logger.LogWarning("Login failed - wrong password for {UserName}", dto.UserName); // לוג שגיאה
                return null;
            }
              
           

            var token = _tokenService.GenerateToken(user.Id, user.Email, user.FirstName, user.LastName, user.UserName, user.RoleUser);
            var expiryMinutes = _configuration.GetValue<int>("JwtSettings:ExpiryMinutes", 60);
            _logger.LogInformation("User {UserId} authenticated successfully", user.Id);
            return new LoginResponseDto
            {
                Token = token,
                TokenType = "Bearer",
                ExpiresIn = expiryMinutes * 60, 
                User = MapToResponseDto(user)
            };

        }

        private static GetUsers MapToResponseDto(User user)
        {
            return new GetUsers
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.RoleUser.ToString()
            };
        }
        private static string HashPassword(string password)
        {
           
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

        }

        
      


    }


}

