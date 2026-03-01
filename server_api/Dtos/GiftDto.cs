using server_api.Models;
using System.ComponentModel.DataAnnotations;

namespace server_api.Dtos
{
    public class GiftDto
    {
        public class Get()
        {
            public int Id { get; set; }
            public string GiftName { get; set; } = string.Empty;
            public string? Description { get; set; }
            public decimal TicketPrice { get; set; }
            public string? ImageUrl { get; set; }

            public int CategoryId { get; set; }

            public string? CategoryName { get; set; } 

            public int DonorId { get; set; }
            public bool IsDrawn { get; set; }
            public string? WinnerName { get; set; }
            public string? DonorName { get; set; }
        }

        public class CreatUpdate()
        {
            [Required]
            [MaxLength(50)]
            public string GiftName { get; set; } = string.Empty;

            [MaxLength(50)]
            public string? Description { get; set; }

            [Required]
            [Range(1, 1000)]
            public decimal TicketPrice { get; set; }

            [Required]
            public string ImageUrl { get; set; } = string.Empty;

            [Required]
            public int CategoryId { get; set; }

            [Required]
            public int DonorId { get; set; }

        }
        public class MostPurchasedGiftDto
        {
            public int? GiftId { get; set; }
            public string GiftName { get; set; }
            public decimal TicketPrice { get; set; }
            public int TotalQuantity { get; set; }
            public bool IsDrawn { get; set; }
        }
        public class CategoryDto
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
        public class AfterRandom
        {

            public int Id { get; set; }
            [Required]
            [MaxLength(50)]
            public string GiftName { get; set; } = string.Empty;

            [MaxLength(50)]
            public string? Description { get; set; }

            [Required]
            [Range(1, 1000)]
            public decimal TicketPrice { get; set; }

            [MaxLength(200)]
            public string? ImageUrl { get; set; }

            [Required]
            public int CategoryId { get; set; }

            [Required]
            public int DonorId { get; set; }
            public bool IsDrawn { get; set; }
            public string? WinnerName { get; set; }
            public int? WinnerUserId { get; set; }
         public string? CategoryName { get; set; }
        }
    }
}

