using server_api.Dtos;
using server_api.Models;

namespace Chinese_sale_Api.Interfaces
{
    public interface IBasketService
    {
        Task AddToBasket(BasketDto basketDto);
        Task<int> Checkout(CheckoutDto checkoutDto);
        Task<Basket?> GetBasketByUserIdAsync(int userId);
        Task RemoveBasketItemAsync(int basketItemId);
    }
}
