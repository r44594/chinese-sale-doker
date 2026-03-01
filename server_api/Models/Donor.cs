using System.ComponentModel.DataAnnotations;

namespace server_api.Models
{
    public class Donor
    {
        public int Id { get; set; }

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
        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }
}
