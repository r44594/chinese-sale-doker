using Chinese_sale_Api.Interfaces;
using server_api.Dtos;
using server_api.Models;
namespace Chinese_sale_Api.Services

{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository Repository;
        private readonly ILogger<OrderService> _logger;
        public BasketService(IBasketRepository Repository, ILogger<OrderService> logger)
        {
            this.Repository = Repository;
            _logger = logger;
        }
        // הוספה לסל + בדיקה אם כבר יש זוכה
        public async Task AddToBasket(BasketDto basketDto)
        {
            _logger.LogInformation("Adding to basket for user {UserId}", basketDto.UserId);
            var basket = await Repository.GetBasketByUserIdAsync(basketDto.UserId);
            // אם אין סל, יוצרים חדש
            if (basket == null)
            {
                _logger.LogInformation("No basket found, creating new basket for user {UserId}", basketDto.UserId); // Logging
                basket = new Basket { UserId = basketDto.UserId };
                await Repository.AddBasketAsync(basket);
            }
            basket.BasketItem ??= new List<BasketItem>();
            // שולפים את המתנה
            var gift = await Repository.GetGiftByIdAsync(basketDto.giftId);
            if (gift == null)
            {
                _logger.LogWarning("Gift with ID {GiftId} not found", basketDto.giftId); // Warning Logging
                throw new ArgumentException("המתנה לא קיימת במערכת.");
            }
            if (gift.IsDrawn == true)
            {
                _logger.LogWarning("Gift {GiftName} already drawn", gift.GiftName); // Warning Logging
                throw new ArgumentException($"לא ניתן להוסיף לסל – המתנה '{gift.GiftName}' כבר הוגרלה.");

            }




            // אם הפריט כבר בסל, מגדילים את הכמות
            var basketItem = basket.BasketItem.FirstOrDefault(bi => bi.GiftId == basketDto.giftId);
            if (basketItem != null)
            {
                basketItem.Quantity += basketDto.quantity;
                await Repository.UpdateBasketItemAsync(basketItem);
                _logger.LogInformation("Increased quantity of gift {GiftId} in basket", basketDto.giftId); // Logging
            }
            else
            {
                basketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    GiftId = basketDto.giftId,
                    Quantity = basketDto.quantity
                };
                await Repository.AddBasketItemAsync(basketItem);
                _logger.LogInformation("Added gift {GiftId} to basket", basketDto.giftId); // Logging
            }
        }

        // פונקציה אחת לרכישה – Checkout
        public async Task<int> Checkout(CheckoutDto checkoutDto)
        {
            _logger.LogInformation("Checkout initiated for user {UserId}", checkoutDto.UserId);
            if (string.IsNullOrEmpty(checkoutDto.CreditCard) || checkoutDto.CreditCard.Length != 16)
            {
                throw new ArgumentException("כרטיס אשראי לא תקין - חייב להכיל 16 ספרות.");
            }
            var basket = await Repository.GetBasketByUserIdAsync(checkoutDto.UserId);
            if (basket == null || !basket.BasketItem.Any())
            {
                _logger.LogWarning("Checkout failed: empty basket for user {UserId}", checkoutDto.UserId); // Warning Logging
                throw new ArgumentException("לא ניתן לבצע רכישה עם סל ריק");
            }

            // 🔴 בדיקה חוזרת – אם אחת המתנות כבר הוגרלה
            //foreach (var item in basket.BasketItem)
            //{
            //    if (item.Gift.IsDrawn)
            //    {
            //        _logger.LogWarning("Checkout failed: gift {GiftName} already drawn", item.Gift.GiftName); // Warning Logging
            //        throw new ArgumentException($"לא ניתן לבצע רכישה – המתנה '{item.Gift.GiftName}' כבר הוגרלה");
            //    }

            //}

          
            decimal totalPrice = basket.BasketItem.Sum(bi => bi.Gift.TicketPrice * bi.Quantity);

            // יוצרים הזמנה
            var order = new Order
            {
                UserId = checkoutDto.UserId,
                OrderTime = DateTime.UtcNow,
                TotalAmount = totalPrice,
            };
            await Repository.AddOrderAsync(order);
            _logger.LogInformation("Order {OrderId} created for user {UserId}", order.Id, checkoutDto.UserId); // Logging

            // יוצרים פריטי הזמנה
            var orderItems = basket.BasketItem.Select(bi => new OrderItem
            {
                OrderId = order.Id,
                GiftId = bi.GiftId,
                Quantity = bi.Quantity,
                PriceAtPurchase = bi.Gift.TicketPrice
            }).ToList();
            await Repository.AddOrderItemsAsync(orderItems);
            _logger.LogInformation("Order items added for order {OrderId}", order.Id); 
                                                                                       
            await Repository.ClearBasketAsync(basket.BasketItem);
            _logger.LogInformation("Basket cleared for user {UserId}", checkoutDto.UserId);

            return order.Id;
        }
        //תכתוב לי פוןנקציה זו-  public async Task<Basket?> GetBasketByUserIdAsync(int userId)
        public async Task<Basket?> GetBasketByUserIdAsync(int userId)
        {
            _logger.LogInformation("Fetching basket for user {UserId}", userId); 
            var basket = await Repository.GetBasketByUserIdAsync(userId);
            if (basket == null)
            {
                _logger.LogWarning("No basket found for user {UserId}", userId); 
            }
            return basket;

        }

        public async Task RemoveBasketItemAsync(int basketItemId)
        {
            _logger.LogInformation("Service: Checking if basket item {Id} exists", basketItemId);

            // לוגיקה: בדיקה אם השורה קיימת בסל
            // הערה: עדיף להשתמש בפונקציה שמחפשת ב-BasketItems ולא ב-Gifts
            var exists = await Repository.GetBasketItemByIdAsync(basketItemId);
            if (exists == null)
            {
                _logger.LogWarning("Service: Basket item {Id} not found", basketItemId);
                throw new ArgumentException("הפריט לא נמצא בסל, ייתכן שכבר נמחק.");
            }

            // שליחת ה-ID ל-Repository (כי זה מה שהוא מצפה לקבל)
            await Repository.DeleteBasketItemAsync(basketItemId);

            _logger.LogInformation("Service: Item {Id} deleted successfully", basketItemId);
        }

    }
}

