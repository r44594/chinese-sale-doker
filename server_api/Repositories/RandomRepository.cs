using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Interfaces;
using server_api.Models;

namespace server_api.Repositories
{
    public class RandomRepository : IRandomRepository
    {
        private readonly SellContext context;
        public RandomRepository(SellContext context)
        {
            this.context = context;
        }
        public async Task<List<IGrouping<Gift, OrderItem>>> DrawLottery()
        {
            var result = await context.OrderItems
                .Include(oi => oi.Gift)
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.User)
                .Where(oi => oi.Order.User != null)
                .GroupBy(oi => oi.Gift!)
                .ToListAsync();
            return result;
        }
        public async Task<List<Gift>> GetGiftsWithWinnersAsync()
        {
            return await context.Gifts
                .Include(g => g.WinnerUser) 
                 //שמתי לבנתיים כדי שההגרלה "כביכול" תעבוד לי
                //.Where(g => g.IsDrawn==true)       
                .ToListAsync();
        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
 
        public async Task<decimal> GetTotalIncomeAsync()
        {
            return await context.Orders.SumAsync(o => o.TotalAmount);
        }
 

        public async Task<List<OrderItem>> GetOrderItemsByGiftIdAsync(int giftId)
        {
            return await context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.User)
                .Include(oi => oi.Gift)
                .Where(oi => oi.GiftId == giftId && oi.Order.User != null)
                .ToListAsync();
        }

        public async Task<Gift> GetGiftByIdAsync(int giftId)
        {
            return await context.Gifts.FindAsync(giftId);
        }
        
        public async Task ResetLotteryTest()
        {
            var gifts = await context.Gifts.ToListAsync();
            foreach (var g in gifts)
            {
                g.IsDrawn = false;
                g.WinnerUserId = null;
            }
            await context.SaveChangesAsync();
        }
    }
}
