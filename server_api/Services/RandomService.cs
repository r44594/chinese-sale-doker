using Chinese_sale_Api.Services;
using Microsoft.Extensions.Caching.Distributed;
using server_api.Dtos;
using server_api.Interfaces;
using server_api.Models;
using System;
using System.Text.Json;

namespace server_api.Services
{
    public class RandomService : IRandomService
    {
        private readonly IRandomRepository repository;
        private readonly Random _random = new Random();
        private readonly ILogger<RandomService> _logger;
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };
        public RandomService(IRandomRepository repository, ILogger<RandomService> logger , IDistributedCache cache)
        {
            this.repository = repository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<List<RandomDto>> DrawLottery()
        {
              //בפונקציה זו אני שמתי את השורות בהערות לבנתים כדי שההגרלה "כביכול" תעבוד לי! ל
            try
            {
                _logger.LogInformation("מתחיל תהליך הגרלת הפרסים");

                var groupedOrderItems = await repository.DrawLottery();
                
                if (groupedOrderItems == null || !groupedOrderItems.Any())
                {
                    _logger.LogWarning("ניסיון להריץ הגרלה ללא נתוני מכירה");
                    
                    throw new Exception("לא נמצאו פריטים להגרלה. וודא שבוצעו רכישות במערכת.");
                }

               
                var results = new List<RandomDto>();
                var random = new Random();
                foreach (var giftGroup in groupedOrderItems)
                {
                    var gift = giftGroup.Key;

                 
                    if (gift == null) continue;

                   
                    if (gift.IsDrawn)
                    {
                        _logger.LogInformation($"המתנה '{gift.GiftName}' כבר הוגרלה בעבר, מדלג.");
                        continue;
                    }

                    var lotteryPool = new List<User>();

                    foreach (var item in giftGroup)
                    {
                        
                        if (item.Order?.User == null)
                        {
                            _logger.LogError($"נמצאה הזמנה (ID: {item.OrderId}) ללא משתמש תקין!");
                            continue;
                        }

                        
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            lotteryPool.Add(item.Order.User);
                        }
                    }

                    
                    if (!lotteryPool.Any())
                    {
                        _logger.LogWarning($"למתנה '{gift.GiftName}' אין רוכשים, לא ניתן לבצע הגרלה.");
                        continue;
                    }

                 
                    var winner = lotteryPool[random.Next(lotteryPool.Count)];

              
                  gift.IsDrawn = true;
                    gift.WinnerUserId = winner.Id;
                    gift.WinnerUser = winner;

                    results.Add(new RandomDto
                    {
                        GiftId = gift.Id,
                        GiftName = gift.GiftName,
                        WinnerName = $"{winner.FirstName} {winner.LastName}"
                    });
                }

               
                if (!results.Any())
                {
                    throw new Exception("תהליך ההגרלה הסתיים ללא זוכים חדשים.");
                }

              await repository.SaveAsync();
                // // כאן הוספתי: ברגע שבוצעה הגרלה חדשה, אנחנו מוחקים את הדוח הישן מה-Cache
                // // כדי שהמשתמשים יראו את הזוכים החדשים בפעם הבאה שיבקשו דוח
                await _cache.RemoveAsync("winners_report");
                _logger.LogInformation("הגרלת הפרסים הסתיימה בהצלחה ונשמרה במסד הנתונים");
                //לבנתיים רק כדי לבדוק שההגרלה עובדת לי!
                //_logger.LogInformation("בדיקה: השמירה בוטלה כדי לא לחסום את המכירה");
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה קריטית במהלך ביצוע ההגרלה");
               
                throw new Exception($"נכשלה הרצת ההגרלה: {ex.Message}");
            }
        }
        public async Task<List<RandomDto>> GetWinnersReportAsync()
        {
    
            _logger.LogInformation("Generating winners report");
            string cacheKey = "winners_report";
            var cachedReport = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedReport))
            {
                _logger.LogInformation("Returning winners report from Redis cache");
                return JsonSerializer.Deserialize<List<RandomDto>>(cachedReport, _jsonOptions);
            }
            var gifts = await repository.GetGiftsWithWinnersAsync();

            var result = new List<RandomDto>();

            foreach (var g in gifts)
            {
                var dto = new RandomDto
                {
                    GiftId = g.Id,
                    GiftName = g.GiftName,
                    WinnerName = g.WinnerUser != null
             ? g.WinnerUser.FirstName + " " + g.WinnerUser.LastName
             : "אין זוכה עדיין"
                };

                result.Add(dto);
            }
            // // שומרים את התוצאה ב-Redis ל-24 שעות (כי זוכים לא משתנים כל רגע)
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(24));
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result, _jsonOptions), cacheOptions);
            return result;
        }

        public async Task<RandomIncomeDto> GetTotalIncomeAsync()
        {
            _logger.LogInformation("Fetching total income");
            return new RandomIncomeDto
            {
                TotalIncome = await repository.GetTotalIncomeAsync()
            };
        }
        
    }
}

