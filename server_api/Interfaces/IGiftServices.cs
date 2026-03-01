using server_api.Dtos;
using server_api.Models;
using static server_api.Dtos.GiftDto;

namespace server_api.Interfaces
{
    public interface IGiftServices
    {

        Task<List<GiftDto.Get>> GetAllGifts();
        Task<GiftDto.Get> GetGiftById(int id);
        Task<Gift> AddGift(GiftDto.CreatUpdate dto);
        Task DeleteGift(int id);
        Task<Gift> UpdateGift(int id, GiftDto.CreatUpdate dto);
        Task<Donor> GetDonorByGiftId(int id);
        Task<List<GiftDto.Get>> SearchGiftsByName(string giftName);
        Task<List<GiftDto.Get>> SearchGiftsByDonorNameAsync(string donorName);
        Task<List<GiftDto.Get>> GetGiftsByUniqueBuyerCountAsync(int buyerCount);
        Task<List<GiftDto.Get>> GetGiftsByCategoryNameAsync(string categoryName);
        Task<List<GiftDto.Get>> GetGiftsOrderedByPriceAscAsync();
        Task<List<GiftDto.AfterRandom>> GetAllGiftsAfterRandom();
        Task<List<CategoryDto>> GetAllCategories();
    }
}
