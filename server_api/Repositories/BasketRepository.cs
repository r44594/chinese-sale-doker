using Chinese_sale_Api.Interfaces;

using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Dtos;
using server_api.Models;

namespace Chinese_sale_Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly SellContext context;
        public BasketRepository(SellContext context)
        {
            this.context = context;

        }

        //לוקחת סל → בודקת → מחשבת מחיר → יוצרת הזמנה → יוצרת פריטי הזמנה → מרוקנת סל → מחזירה מזהה הזמנה
        public async Task<Basket?> GetBasketByUserIdAsync(int userId)
        {
            return await context.Baskets
                .Include(b => b.BasketItem)
                .ThenInclude(bi => bi.Gift)
                .FirstOrDefaultAsync(b => b.UserId == userId);
        }
        public async Task<BasketItem> GetBasketItemByIdAsync(int basketItemId)
        {
            return await context.BasketItems.FindAsync(basketItemId);
        }
        public async Task<Gift?> GetGiftByIdAsync(int giftId)
        {
            return await context.Gifts.FirstOrDefaultAsync(g => g.Id == giftId);
        }

        public async Task AddBasketAsync(Basket basket)
        {
            context.Baskets.Add(basket);
            await context.SaveChangesAsync();
        }

        public async Task AddBasketItemAsync(BasketItem item)
        {
            context.BasketItems.Add(item);
            await context.SaveChangesAsync();
        }

        public async Task UpdateBasketItemAsync(BasketItem item)
        {
            context.BasketItems.Update(item);
            await context.SaveChangesAsync();
        }

        // BasketRepository.cs
        public async Task DeleteBasketItemAsync(int basketItemId)
        {
            var item = await context.BasketItems.FindAsync(basketItemId);
            if (item != null)
            {
                context.BasketItems.Remove(item);
                await context.SaveChangesAsync();
            }
           
        }
        public async Task AddOrderAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        public async Task AddOrderItemsAsync(IEnumerable<OrderItem> items)
        {
            context.OrderItems.AddRange(items);
            await context.SaveChangesAsync();
        }
    
        public async Task ClearBasketAsync(IEnumerable<BasketItem> items)
        {
            context.BasketItems.RemoveRange(items);
            await context.SaveChangesAsync();
        }
    }

}
    

