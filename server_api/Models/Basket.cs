using System.ComponentModel.DataAnnotations;



namespace server_api.Models
{
    public class Basket
    {
        public enum BasketStatus
        {
            Open,    
            OrderCompleted,
            Closed    
        }
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<BasketItem> BasketItem { get; set; } = new List<BasketItem>();
        public decimal TotalPrice => BasketItem.Sum(i => i.Quantity * i.Gift.TicketPrice);
        public BasketStatus Status { get; set; } = BasketStatus.Open;
    }

}
