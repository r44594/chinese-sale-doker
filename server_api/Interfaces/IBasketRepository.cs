using server_api.Dtos;
using server_api.Models;

namespace Chinese_sale_Api.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketByUserIdAsync(int userId);
        Task<Gift?> GetGiftByIdAsync(int giftId);

        Task AddBasketAsync(Basket basket);
        Task AddBasketItemAsync(BasketItem item);
        Task UpdateBasketItemAsync(BasketItem item);
        Task DeleteBasketItemAsync(int basketItemId);

        Task AddOrderAsync(Order order);
        Task AddOrderItemsAsync(IEnumerable<OrderItem> items);
        Task ClearBasketAsync(IEnumerable<BasketItem> items);
        Task<BasketItem> GetBasketItemByIdAsync(int basketItemId);
        }
}
