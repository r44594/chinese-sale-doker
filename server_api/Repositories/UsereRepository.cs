using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Dtos;
using server_api.Interfaces;
using server_api.Models;

using static server_api.Dtos.UserDto;

namespace server_api.Repositories
{
    public class UsereRepository : IUsereRepository
    {
        private readonly SellContext _context;


        public UsereRepository(SellContext context)
        {
            _context = context;
        }
        public async Task<User> register(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        // ✅ לוגין לפי UserName + Password
        public async Task<User?> login(LoginDto dto)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.UserName);
        }
        public async Task<User?> GetByUserName(string userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }


      
    }


}
