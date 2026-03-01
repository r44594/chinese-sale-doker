using server_api.Dtos;
using server_api.Interfaces;
using server_api.Models;
using System.Numerics;
using System.Linq;
using Chinese_sale_Api.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace server_api.Services
{
    
    public class DonorServices : IDonorServices
    {
        private readonly IDonorRepositories _repository;
        private readonly ILogger<DonorServices> _logger;
        private readonly IDistributedCache _cache; // הוספה: שדה ל-Redis
        private const string DonorsCacheKey = "all_donors_list"; // הוספה: מפתח למטמון
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };
        public DonorServices(IDonorRepositories repositories, ILogger<DonorServices> logger, IDistributedCache cache)
        {
            _repository = repositories;
            _logger = logger;
            _cache = cache;
        }
     
        public async Task AddDonor(DonorDto.CreatUpdateDonor dto)
        {
            _logger.LogInformation("Adding new donor: {FirstName} {LastName}", dto.FirstName, dto.LastName); // לוג
            var donor = new Donor()
            {

                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone
            };
            await _repository.AddDonor(donor);
            await _cache.RemoveAsync(DonorsCacheKey);
        }

        //פונקציה שמחזירה לי אץ כל התורמים עם רשימת המתנות של כל תורם

        public async Task<List<GetDonor>> GetAllDonors()
        {
            var cachedDonors = await _cache.GetStringAsync(DonorsCacheKey);
            if (!string.IsNullOrEmpty(cachedDonors))
            {
                _logger.LogInformation("Returning donors from Redis cache");
                return JsonSerializer.Deserialize<List<GetDonor>>(cachedDonors, _jsonOptions);
            }
            _logger.LogInformation("Fetching all donors from Database");
            var donors = await _repository.GetAllDonors();

            var result = donors.Select(d => new GetDonor
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                Phone = d.Phone,
                Gifts = d.Gifts.Select(g => new DonorDto.GiftDonor
                {
                    Id = g.Id,
                    GiftName = g.GiftName,
                    TicketPrice = g.TicketPrice,
                    CountOfSale = g.OrderItems != null ? g.OrderItems.Count : 0
                }).ToList()
            }).ToList();

            // הוספה: שמירה ב-Redis ל-30 דקות
            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) };
            await _cache.SetStringAsync(DonorsCacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return result;
        }

        public async Task<GetDonor> GetByDonorName(string donorName)
        {
            _logger.LogInformation("Fetching donor by name: {DonorName}", donorName);

            var donor = await _repository.GetByDonorName(donorName);
            if (donor == null)
            {
                _logger.LogWarning("Donor not found: {DonorName}", donorName);
                throw new Exception("תורם זה לא קיים במערכת");
            }

            return new GetDonor
            {
                Id = donor.Id,
                FirstName = donor.FirstName,
                LastName = donor.LastName,
                Email = donor.Email,
                Phone = donor.Phone,
                Gifts = donor.Gifts.Select(g => new DonorDto.GiftDonor
                {
                    Id = g.Id,
                    GiftName = g.GiftName,
                    TicketPrice = g.TicketPrice,
                    CountOfSale = g.OrderItems != null ? g.OrderItems.Count : 0
                }).ToList()
            };
        }


        public async Task<GetDonor> GetDonorById(int id)
        {
            _logger.LogInformation("Fetching donor by ID: {Id}", id);

            var donor = await _repository.GetDonorById(id);
            if (donor == null)
            {
                _logger.LogWarning("Donor not found with ID: {Id}", id);
                throw new Exception("תורם זה לא קיים במערכת");
            }

            return new GetDonor
            {
                Id = donor.Id,
                FirstName = donor.FirstName,
                LastName = donor.LastName,
                Email = donor.Email,
                Phone = donor.Phone,
                Gifts = donor.Gifts.Select(g => new DonorDto.GiftDonor
                {
                    Id = g.Id,
                    GiftName = g.GiftName,
                    TicketPrice = g.TicketPrice,
                    CountOfSale = g.OrderItems != null ? g.OrderItems.Count : 0
                }).ToList()
            };
        }


        public async Task DeleteDonor(int id)
        {
            _logger.LogInformation("Deleting donor with ID: {Id}", id);
            var donor = await _repository.GetDonorById(id);
            if (donor == null)
            {
                _logger.LogWarning("Donor not found for deletion, ID: {Id}", id);
                throw new Exception("תורם זה לא קיים במערכת");
            }
            if (donor.Gifts.Any(g => g.OrderItems != null && g.OrderItems.Count > 0))
            {
                _logger.LogWarning("Cannot delete donor ID: {Id} - tickets have already been purchased", id);
                throw new Exception("לא ניתן לבטל תרומה/למחוק תורם - כבר נרכשו כרטיסים למתנות שלו!");
            }
            await _repository.DeleteDonor(donor);
            await _cache.RemoveAsync(DonorsCacheKey);
        }


        public async Task<Donor> UpdateDonor(int id, DonorDto.CreatUpdateDonor dto)
        {
            _logger.LogInformation("Updating donor with ID: {Id}", id);
            var donor = await _repository.GetDonorById(id);
            if (donor == null)
            {
                _logger.LogWarning("Donor not found for update, ID: {Id}", id);
                throw new Exception("תורם זה לא קיים במערכת");

            }
            donor.FirstName = dto.FirstName;
            donor.LastName = dto.LastName;
            donor.Email = dto.Email;
            donor.Phone = dto.Phone;

            await _repository.UpdateDonor(donor);
            await _cache.RemoveAsync(DonorsCacheKey);
            return donor;
        }
        public async Task<List<GetDonor>> GetByDonorEmail(string email)
        {
            _logger.LogInformation("Fetching donors by email: {Email}", email);

            var donors = await _repository.GetByDonorEmail(email);

            return donors.Select(d => new GetDonor
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                Phone = d.Phone,
                Gifts = d.Gifts.Select(g => new DonorDto.GiftDonor
                {
                    Id = g.Id,
                    GiftName = g.GiftName,
                    TicketPrice = g.TicketPrice,
                    CountOfSale = g.OrderItems != null ? g.OrderItems.Count : 0
                }).ToList()
            }).ToList();
        }



        public async Task<List<GetDonor>> GetByDonorGift(string giftName)
        {
            _logger.LogInformation("Fetching donors by gift name: {GiftName}", giftName);

            var donors = await _repository.GetByDonorGift(giftName);

            return donors.Select(d => new GetDonor
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                Phone = d.Phone,
                Gifts = d.Gifts.Select(g => new DonorDto.GiftDonor
                {
                    Id = g.Id,
                    GiftName = g.GiftName,
                    TicketPrice = g.TicketPrice,
                    CountOfSale = g.OrderItems != null ? g.OrderItems.Count : 0
                }).ToList()
            }).ToList();
        }


    }

}



