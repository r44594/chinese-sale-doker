using System.ComponentModel.DataAnnotations;

namespace server_api.Models
{
    public class Gift
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string GiftName { get; set; }
        [MaxLength(50)]
        public string? Description { get; set; }
        [Required]
        [Range(1,1000)]
       public decimal TicketPrice { get; set; }
        
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        [Required]
        public int DonorId { get; set; }
        public Donor Donor { get; set; } = null!;

        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
       
        public bool IsDrawn { get; set; } = false;
        public int? WinnerUserId { get; set; }
        public User? WinnerUser { get; set; }

    }

}
