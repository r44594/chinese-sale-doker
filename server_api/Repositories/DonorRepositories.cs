using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Interfaces;
using server_api.Models;
using System.ComponentModel;

namespace server_api.Repositories
{
    public class DonorRepositories : IDonorRepositories
    {
        private readonly SellContext _context;
        public DonorRepositories(SellContext context)
        {
            _context = context;
        }
        
        public async Task AddDonor(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Donor>> GetAllDonors()
        {
            return await _context.Donors.Include(x => x.Gifts).ThenInclude(g => g.OrderItems).ToListAsync();
        }
        public async Task<Donor?> GetDonorById(int id)
        {
            var donor = await _context.Donors
                .Include(d => d.Gifts)           
            .ThenInclude(g => g.OrderItems)
            .SingleOrDefaultAsync(d => d.Id == id);
            return donor;

        }
        public async Task DeleteDonor(Donor donor)
        {
            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
        }
        public async Task<Donor> UpdateDonor(Donor donor)
        {
            _context.Donors.Update(donor);
            await _context.SaveChangesAsync();
            return donor;
        }
        //לשים לב פונקציה זו יכולה להחזיר רק שם אחד כי הוא שם יחודי למשתמש
        public async Task<Donor?> GetByDonorName(string name)
        {
            return await _context.Donors
                .Include(d => d.Gifts)             
                    .ThenInclude(g => g.OrderItems) 
                .FirstOrDefaultAsync(x => x.FirstName.Contains(name) || x.LastName.Contains(name));
        }

        public async Task<List<Donor>> GetByDonorGift(string giftName)
        {
            return await _context.Donors
                .Include(d => d.Gifts)
                .Where(d => d.Gifts.Any(g => g.GiftName.Contains(giftName)))
                .ToListAsync();
        }

        public async Task<List<Donor>> GetByDonorEmail(string email)
        {
            return await _context.Donors
                                 .Where(x => x.Email.Contains(email) )
                                 .ToListAsync();
        }
      
    } 
}
