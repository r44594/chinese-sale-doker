namespace Chinese_sale_Api.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public string GiftName { get; set; } = "";
        public decimal TicketPrice { get; set; }
    }
    public class BuyerOrderDto
    {
        public int OrderId { get; set; }         
        public DateTime OrderDate { get; set; }   
        public int UserId { get; set; }
        public DateTime OrderTime { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int GiftId { get; set; }        
        public string GiftName { get; set; } = ""; 

    }

}
