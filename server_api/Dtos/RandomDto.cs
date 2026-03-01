namespace server_api.Dtos
{
    public class RandomDto
    {
        public int GiftId { get; set; }
        public string GiftName { get; set; }
      
        public string WinnerName { get; set; }
    }
    public class RandomIncomeDto
    {
        public decimal TotalIncome { get; set; }
    }

}
