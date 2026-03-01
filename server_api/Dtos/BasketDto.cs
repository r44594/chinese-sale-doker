using System.ComponentModel.DataAnnotations;

namespace server_api.Dtos
{
    public class BasketDto
    {
        [Required] 
        public int giftId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int quantity { get; set; } = 1;

    }
    public class CheckoutDto
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "כרטיס אשראי חייב להכיל 16 ספרות")]
        public string CreditCard { get; set; }
    }
}
