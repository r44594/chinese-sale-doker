using server_api.Models;

namespace server_api.Interfaces
{
    public interface IRandomRepository
    {
        Task<List<IGrouping<Gift, OrderItem>>> DrawLottery();
        Task SaveAsync();
        Task<List<Gift>> GetGiftsWithWinnersAsync();
        Task<decimal> GetTotalIncomeAsync();
        Task<List<OrderItem>> GetOrderItemsByGiftIdAsync(int giftId);
        Task<Gift> GetGiftByIdAsync(int giftId);
    }

}
