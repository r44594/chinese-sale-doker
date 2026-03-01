using Chinese_sale_Api.Dtos;
using Chinese_sale_Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using server_api.Data;
using server_api.Dtos;
using server_api.Models;

namespace Chinese_sale_Api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SellContext context;
        public OrderRepository(SellContext context)
        {
            this.context = context;
        }
        public async Task<int> GetNumberOfTickets(int giftId)
        {
            var sum = await context.OrderItems
                .Where(od => od.GiftId == giftId)
                .SumAsync(od => od.Quantity);
            return sum;
        }
        //o	אפשרות בחירת מיון, לפי המתנה היקרה ביותר
        public async Task<List<OrderDto>> OrderByTotalPrice()
        {
            var orders = await context.Orders
               .Include(o => o.OrderItem)
               .ThenInclude(oi => oi.Gift)
               .ToListAsync();

            var ordersDto = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                Items = o.OrderItem.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    GiftName = oi.Gift.GiftName,
                    TicketPrice = oi.Gift.TicketPrice
                }).ToList()
            }).ToList();

            return ordersDto;

        }
        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            return await context.OrderItems
                .Include(o => o.Gift)
                .ToListAsync();
        }
        public async Task<List<Order>> GetBuyerDetails()
        {
            return await context.Orders
                .Include(o => o.User)         
                .Include(o => o.OrderItem)    
                    .ThenInclude(oi => oi.Gift) 
                .ToListAsync();
        }
    }
}
