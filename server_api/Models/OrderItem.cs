using System.ComponentModel.DataAnnotations;

namespace server_api.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        [Required]
        public int? GiftId { get; set; }
        public Gift? Gift { get; set; } = null!;
        [Required]
        [MaxLength(1000)]
        public int Quantity { get; set; }
        [Required]
        [Range(1,1000)]
        public decimal PriceAtPurchase { get; set; }

    }
}
