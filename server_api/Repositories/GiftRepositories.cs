using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using server_api.Data;
using server_api.Interfaces;
using server_api.Models;
using System.ComponentModel;
using static server_api.Dtos.GiftDto;

namespace server_api.Repositories
{
    public class GiftRepositories : IGiftRepositories
    {
        private readonly SellContext _context;
        public GiftRepositories(SellContext context)
        {
            _context = context;
        }


        public async Task<List<Gift>> GetAllGifts()
        {
            return await _context.Gifts
            .Include(g => g.Category)
           .Include(g => g.Donor)
           .Include(g => g.OrderItems)
           .Include(g => g.WinnerUser)
            .ToListAsync();
        }

        public async Task<Gift?> GetGiftById(int id)
        {
            var gift = await _context.Gifts
                .Include(g => g.Donor) 
                .Include(g => g.Category)
                .Include(g => g.WinnerUser)
                .SingleOrDefaultAsync(d => d.Id == id);
            return gift;
        }
        public async Task AddGift(Gift gift)
        {
            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGift(Gift gift)
        {
            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
        }

        public async Task<Gift?> UpdateGift(Gift gift)
        {
            _context.Gifts.Update(gift);
            await _context.SaveChangesAsync();
            return gift;
        }
       
        public async Task<Donor?> GetDonorByGiftId(int giftId)
        {
            var gift = await _context.Gifts
                 .Include(g => g.Donor)
                 .FirstOrDefaultAsync(g => g.Id == giftId);
            return gift?.Donor;
        }

        //חיפוס מתנות לפי שם מסויים שמתקבל בפונקציה
        public async Task<List<Gift>> SearchGiftsByName(string giftName)
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .Where(g => g.GiftName.Contains(giftName))
                .ToListAsync();
        }
        //חיפוס מתנה לפי תורם--==--

        public async Task<List<Gift>> SearchGiftsByDonorName(string donorName)
        {
            return await _context.Gifts
                .Include(g => g.Donor)
               .Where(g => g.Donor != null && g.Donor.FirstName.Contains(donorName)) 
                .ToListAsync();
        }
        public async Task<List<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Gift)       
                .Include(oi => oi.Order)      
                    .ThenInclude(o => o.User) 
                .ToListAsync();
        }
        public async Task<bool> HasOrders(int giftId)
        {
            return await _context.OrderItems
                .AnyAsync(oi => oi.GiftId == giftId);

        }


        //פונקציה שמחזירה לי רשימת מתנות ע"פ שם של קטגוריה מסוימת
        public async Task<List<Gift>> GetGiftsByCategoryName(string categoryName)
        {
            return await _context.Gifts
            .Include(g => g.Category)
                .Where(g => g.Category != null && g.Category.Name.Contains(categoryName))
                .ToListAsync();
        }

        //פונקציה שמחזירה לי רשימת מתנות של דיטיאו שכל המתנות ממיונות לי מהמחיר הנמוך למחיר הגבוה
        public async Task<List<Gift>> GetGiftsOrderedByPriceAsc()
        {
            return await _context.Gifts
                .OrderBy(g => g.TicketPrice)
                .ToListAsync();
        }
        public async Task<List<OrderItem>> GetGiftsByUniqueBuyerCountAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Gift)
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.User)
                .ToListAsync();
        }

        //פונקציה שמחזירה את כל הקטגוריות שמשתמשת בדיטיאו של הקטגוריה  שיחזיר ראשימה של קטגוריות
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.categories.ToListAsync();
        }








    }

}