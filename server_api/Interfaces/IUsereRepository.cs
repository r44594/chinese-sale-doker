using server_api.Dtos;
using server_api.Models;

namespace server_api.Interfaces
{
    public interface IUsereRepository
    {
        Task<User> register(User user);
        Task<User?> login(LoginDto dto);
        Task<User?> GetByUserName(string userName);

        //Task<List<User>> GetAllUser();
        //Task<User>GetUserById(int id);
        //Task DeleteUser(User user);
        //Task<User> UpdateUser(User user);
    }

}
