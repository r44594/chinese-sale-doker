using server_api.Data;
using server_api.Dtos;
using server_api.Interfaces;
using server_api.Models;
using server_api.Repositories;
using System.Drawing;
using static server_api.Dtos.GiftDto;

namespace server_api.Services
{
    public class GiftServices : IGiftServices
    {
        private readonly IGiftRepositories _repository;
        private readonly ILogger<GiftServices> _logger;
        public GiftServices(IGiftRepositories repositories, ILogger<GiftServices> logger)
        {
            _repository = repositories;
            _logger = logger;
        }
        public async Task<List<GiftDto.Get>> GetAllGifts()
        {
            _logger.LogInformation("Fetching all gifts");
            var gifts = await _repository.GetAllGifts();
            return gifts.Select(g => new GiftDto.Get
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Description = g.Description,
                TicketPrice = g.TicketPrice,
                ImageUrl = g.ImageUrl,
                CategoryId = g.CategoryId,
                //לשים לב בכל מקום שכתוב לי קאגורי לכתוב גם קטגורי-ניים
                CategoryName = g.Category != null ? g.Category.Name : null,
                DonorName = g.Donor?.FirstName + " " + g.Donor?.LastName,
                DonorId = g.DonorId,
                IsDrawn = g.IsDrawn,
                WinnerName = g.IsDrawn && g.WinnerUser != null
                ? g.WinnerUser.FirstName + " " + g.WinnerUser.LastName : null
            }).ToList();
        }

        public async Task<List<GiftDto.AfterRandom>> GetAllGiftsAfterRandom()
        {


            _logger.LogInformation("Fetching all gifts");
            var gifts = await _repository.GetAllGifts();
            return gifts.Select(g => new GiftDto.AfterRandom
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Description = g.Description,
                TicketPrice = g.TicketPrice,
                ImageUrl = g.ImageUrl,
                CategoryId = g.CategoryId,
                DonorId = g.DonorId,
                IsDrawn = g.IsDrawn,
                CategoryName = g.Category != null ? g.Category.Name : null,
                WinnerName = g.IsDrawn && g.WinnerUser != null
                ? g.WinnerUser.FirstName + " " + g.WinnerUser.LastName : null,
                WinnerUserId = g.WinnerUserId
            }).ToList();
        }


        public async Task<GiftDto.Get> GetGiftById(int id)
        {
            _logger.LogInformation("Fetching gift by ID: {Id}", id);
            var gift = await _repository.GetGiftById(id);
            if (gift == null)
            {
                _logger.LogWarning("Gift not found with ID: {Id}", id);
                throw new Exception("מוצר זה לא קיים במערכת!");
            }
            return new GiftDto.Get
            {
                Id = gift.Id,
                GiftName = gift.GiftName,
                Description = gift.Description,
                TicketPrice = gift.TicketPrice,
                ImageUrl = gift.ImageUrl,
                CategoryId = gift.CategoryId,
                CategoryName = gift.Category != null ? gift.Category.Name : null,
                DonorId = gift.DonorId,
                DonorName = gift.Donor?.FirstName,
                IsDrawn = gift.IsDrawn,
                WinnerName = gift.IsDrawn && gift.WinnerUser != null
            ? gift.WinnerUser.FirstName + " " + gift.WinnerUser.LastName
            : null
            };
        }


        public async Task<Gift> AddGift(GiftDto.CreatUpdate dto)
        {
            _logger.LogInformation("Adding new gift: {GiftName}", dto.GiftName);

            var gift = new Gift
            {
                GiftName = dto.GiftName,
                Description = dto.Description,
                TicketPrice = dto.TicketPrice,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                DonorId = dto.DonorId
            };

            await _repository.AddGift(gift);

            _logger.LogInformation("Gift created with ID: {GiftId}", gift.Id);


            return gift;
        }

        public async Task DeleteGift(int id)
        {
            _logger.LogInformation("Deleting gift with ID: {Id}", id);
            var gift = await _repository.GetGiftById(id);
            if (gift == null)
            {
                _logger.LogWarning("Gift not found for deletion, ID: {Id}", id);
                throw new Exception("מוצר זה לא קיים במערכת!");
            }
            if (gift.IsDrawn)
            {
                _logger.LogWarning("Cannot delete gift ID: {Id}, it was already drawn", id);
                throw new InvalidOperationException("לא ניתן למחוק מתנה זו מכיוון שמתנה זו כבר הוגרלה");
            }
            bool isGiftInUse = await _repository.HasOrders(id); // בדיקה אם המתנה בשימוש
            if (isGiftInUse == true)
            {
                _logger.LogWarning("Cannot delete gift, ID: {Id}, it is associated with orders", id);
                throw new Exception("לא ניתן למחוק מתנה זו מכיוון שהיא משוייכת להזמנות קיימות.");
            }
            await _repository.DeleteGift(gift);
        }
        public async Task<Gift> UpdateGift(int id, GiftDto.CreatUpdate dto)
        {
            _logger.LogInformation("Updating gift with ID: {Id}", id);
            var gift = await _repository.GetGiftById(id);
            if (gift == null)
            {
                _logger.LogWarning("Gift not found for update, ID: {Id}", id);
                throw new Exception("מוצר זה לא קיים במערכת!");
            }
            gift.GiftName = dto.GiftName;
            gift.Description = dto.Description;
            gift.TicketPrice = dto.TicketPrice;
            gift.ImageUrl = dto.ImageUrl;
            gift.CategoryId = dto.CategoryId;
            gift.DonorId = dto.DonorId;
            await _repository.UpdateGift(gift);
            return gift;
        }

        //פונקציה שמחזירה לי תורם לפי איידי של מתנה ומחזירה לי דיטיאו של התורם
        public async Task<Donor> GetDonorByGiftId(int id)
        {
            _logger.LogInformation("Fetching donor by gift ID: {Id}", id);
            var donor = await _repository.GetDonorByGiftId(id);
            if (donor == null)
            {
                _logger.LogWarning("Donor not found for gift ID: {Id}", id);
                throw new Exception("תורם זה לא קיים במערכת");
            }
            return donor;

        }



        //חיפוס מתנות לפי שם מסויים שמתקבל בפונקציה
        public async Task<List<GiftDto.Get>> SearchGiftsByName(string giftName)
        {
            _logger.LogInformation("Searching gifts by name: {GiftName}", giftName);
            var gifts = await _repository.SearchGiftsByName(giftName);
            var giftDtos = gifts.Select(g => new GiftDto.Get
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Description = g.Description,
                TicketPrice = g.TicketPrice,
                ImageUrl = g.ImageUrl,
                CategoryName = g.Category != null ? g.Category.Name : null,
                CategoryId = g.CategoryId,

            }).ToList();

            return giftDtos;
        }
        //חיפוס מתנה לפי תורם--==--
        public async Task<List<GiftDto.Get>> SearchGiftsByDonorNameAsync(string donorName)
        {
            _logger.LogInformation("Searching gifts by donor name: {DonorName}", donorName); // לוג מינימלי
            var gifts = await _repository.SearchGiftsByDonorName(donorName);

            var giftDtos = gifts.Select(g => new GiftDto.Get
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Description = g.Description,
                TicketPrice = g.TicketPrice,
                ImageUrl = g.ImageUrl,
                CategoryName = g.Category != null ? g.Category.Name : null,
                CategoryId = g.CategoryId,
                DonorName = g.Donor != null ? $"{g.Donor.FirstName} {g.Donor.LastName}" : "תורם אנונימי"
            }).ToList();

            return giftDtos;
        }

        // פונקציה שמקבלת מספר רוכשים ומחזירה את המתנות שיש להן בדיוק מספר זה של רוכשים שונים
        public async Task<List<GiftDto.Get>> GetGiftsByUniqueBuyerCountAsync(int buyerCount)
        {
            _logger.LogInformation("Fetching gifts with exactly {BuyerCount} unique buyers", buyerCount); // לוג מינימלי
            var orderItems = await _repository.GetAllOrderItemsAsync();

            // קיבוץ לפי מתנה וספירת רוכשים ייחודיים
            var groupedGifts = orderItems
                .Where(oi => oi.Order?.User != null)
                .GroupBy(oi => oi.Gift)
                .Where(g => g.Select(oi => oi.Order!.User!.Id).Distinct().Count() == buyerCount) // סינון לפי מספר רוכשים
                .Select(g => new GiftDto.Get
                {
                    Id = g.Key.Id,
                    GiftName = g.Key.GiftName,
                    Description = g.Key.Description,
                    //CategoryName = g.Category != null ? g.Category.Name : null,
                    TicketPrice = g.Key.TicketPrice,
                    ImageUrl = g.Key.ImageUrl,
                    CategoryId = g.Key.CategoryId,
                    DonorId = g.Key.DonorId
                })
                .ToList();

            return groupedGifts;
        }
        //פונקציה שמחזירה לי רשימת מתנות ע"פ שם של קטגוריה מסוימת
        public async Task<List<GiftDto.Get>> GetGiftsByCategoryNameAsync(string categoryName)
        {
            _logger.LogInformation("Fetching gifts by category name: {CategoryName}", categoryName); // לוג מינימלי
            var gifts = await _repository.GetGiftsByCategoryName(categoryName);

            var giftDtos = gifts.Select(g => new GiftDto.Get
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Description = g.Description,
                TicketPrice = g.TicketPrice,
                ImageUrl = g.ImageUrl,
                CategoryId = g.CategoryId,
                CategoryName = g.Category != null ? g.Category.Name : null,
                DonorId = g.DonorId
            }).ToList();

            return giftDtos;
        }
        //פונקציה שמחזירה לי רשימת מתנות של דיטיאו שכל המתנות ממיונות לי מהמחיר הנמוך למחיר הגבוה
        public async Task<List<GiftDto.Get>> GetGiftsOrderedByPriceAscAsync()
        {
            _logger.LogInformation("Fetching gifts ordered by price ascending");
            var gifts = await _repository.GetGiftsOrderedByPriceAsc();

            var giftDtos = gifts.Select(g => new GiftDto.Get
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Description = g.Description,
                TicketPrice = g.TicketPrice,
                ImageUrl = g.ImageUrl,
                CategoryId = g.CategoryId,
                DonorId = g.DonorId,
                CategoryName = g.Category != null ? g.Category.Name : null
            }).ToList();

            return giftDtos;
        }
        public async Task<List<CategoryDto>> GetAllCategories()
        {

            var categories = await _repository.GetAllCategories();


            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
  

    }




    }



