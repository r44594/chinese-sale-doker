using server_api.Models;
using static server_api.Dtos.GiftDto;

namespace server_api.Interfaces
{
    public interface IGiftRepositories
    {
        Task<List<Gift>> GetAllGifts();
        Task<Gift?> GetGiftById(int id);
        Task AddGift(Gift gift);
        Task DeleteGift(Gift gift);
        Task<Gift?> UpdateGift(Gift gift);
        Task<Donor?> GetDonorByGiftId(int giftId);
        Task<List<Gift>> SearchGiftsByName(string giftName);
        Task<List<Gift>> SearchGiftsByDonorName(string donorName);
        Task<List<OrderItem>> GetAllOrderItemsAsync();
        Task<bool> HasOrders(int giftId);
        Task<List<Gift>> GetGiftsByCategoryName(string categoryName);
        Task<List<Gift>> GetGiftsOrderedByPriceAsc();
        Task<List<OrderItem>> GetGiftsByUniqueBuyerCountAsync();
 
        Task<List<Category>> GetAllCategories();

    }
}
