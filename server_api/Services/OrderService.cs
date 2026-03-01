using Chinese_sale_Api.Dtos;
using Chinese_sale_Api.Interfaces;
using server_api.Dtos;

namespace Chinese_sale_Api.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository repository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository repository, ILogger<OrderService> logger)
        {
            this.repository = repository;
            _logger = logger;
        }
        public async Task<int> GetNumberOfTickets(int giftId)
        {
            _logger.LogInformation("Getting number of tickets for gift ID: {GiftId}", giftId);
            var sum = await repository.GetNumberOfTickets(giftId);
            if (sum <= 0)
            {
                _logger.LogWarning("No tickets found for gift ID: {GiftId}", giftId);
                throw new Exception("אין רכישות למוצר זה");
            }
               
            return sum;
        }
        public async Task<List<OrderDto>> OrderByTotalPrice()
        {
            _logger.LogInformation("Fetching orders sorted by total price");
            var orders = await repository.OrderByTotalPrice();
            if (orders == null || orders.Count == 0)
            {
                _logger.LogWarning("No orders found in the system");
                throw new Exception("אין הזמנות במערכת");
            }
                
            return orders;
        }
     
        public async Task<List<GiftDto.MostPurchasedGiftDto>> GetMostPurchasedGift()
        {
            _logger.LogInformation("Fetching all gifts with purchase statistics");
            var items = await repository.GetAllOrderItems();

            var result = items
                .GroupBy(oi => new { oi.GiftId, oi.Gift.GiftName, oi.Gift.TicketPrice , oi.Gift.IsDrawn })
                .Select(g => new GiftDto.MostPurchasedGiftDto
                {
                    GiftId = g.Key.GiftId,
                    GiftName = g.Key.GiftName,
                    TicketPrice = g.Key.TicketPrice,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    IsDrawn = g.Key.IsDrawn
                })
                .OrderByDescending(x => x.TotalQuantity)
                .ToList(); 

            return result;
        }

        public async Task<List<BuyerOrderDto>> GetBuyerDetails()
        {
            _logger.LogInformation("Fetching buyer details");
            var orders = await repository.GetBuyerDetails();

            
            var result = orders.SelectMany(o => o.OrderItem.Select(oi => new BuyerOrderDto
            {
                OrderId = o.Id,
                OrderDate = o.OrderTime, 
                UserId = o.User?.Id ?? 0,
                FirstName = o.User?.FirstName ?? "לא ידוע",
                LastName = o.User?.LastName ?? "",
                Email = o.User?.Email ?? "",
                Phone = o.User?.Phone ?? "",

                GiftId = oi.GiftId ?? 0,
                GiftName = oi.Gift?.GiftName ?? "ללא שם"
            })).ToList();

            return result;
        }
    }
}
