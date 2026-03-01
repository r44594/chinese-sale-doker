using System.ComponentModel.DataAnnotations;

namespace server_api.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        [Required]
        public int BasketId { get; set; }
        public Basket Basket { get; set; } = null!;
        [Required]
        public int GiftId { get; set; }
        public Gift Gift { get; set; } = null!;
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

    }

}
