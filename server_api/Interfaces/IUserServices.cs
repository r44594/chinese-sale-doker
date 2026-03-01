using Chinese_sale_Api.Dtos;
using server_api.Dtos;
using server_api.Models;


namespace server_api.Interfaces
{
    public interface IUserServices
    {
        Task<GetUsers> register(UserDto.RegisterDto dto);
        Task<LoginResponseDto?> login(LoginDto dto);
        //Task<List<User>> GetAllUser();
        //Task<User> GetUserById(int id);
        //Task DeleteUser(User user);
        //Task<User> UpdateUser(User user);
    }
}
