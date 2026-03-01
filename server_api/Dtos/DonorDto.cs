using server_api.Models;
using System.ComponentModel.DataAnnotations;
using static server_api.Dtos.DonorDto;

namespace server_api.Dtos
{
    public class DonorDto
    {
        public class CreatUpdateDonor{ 
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string LastName { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        [MaxLength(100)]

        public string Email { get; set; } = string.Empty;


        [Required]
        [MaxLength(11)]
        [MinLength(8)]

        public string Phone { get; set; }
           
            
        }
        
        public class GiftDonor
        {
            public int Id { get; set; }
            public string GiftName { get; set; } = string.Empty;
            public decimal TicketPrice { get; set; }
            public int CountOfSale { get; set; }


        }
    }
    public class GetDonor
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public ICollection<GiftDonor> Gifts { get; set; } = new List<GiftDonor>();
    }
}
