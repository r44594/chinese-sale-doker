using Chinese_sale_Api.Dtos;
using server_api.Dtos;

namespace Chinese_sale_Api.Interfaces
{
    public interface IOrderService
    {
        Task<int> GetNumberOfTickets(int giftId);
        Task<List<OrderDto>> OrderByTotalPrice();
        Task<List<GiftDto.MostPurchasedGiftDto>> GetMostPurchasedGift();
        Task<List<BuyerOrderDto>> GetBuyerDetails();

    }
}
