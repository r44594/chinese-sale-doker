using server_api.Repositories;
using System.ComponentModel.DataAnnotations;

namespace server_api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public DateTime OrderTime { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public decimal TotalAmount {  get; set; }
        [Required]
        [MinLength(1)]
        public ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
        
    }
}
