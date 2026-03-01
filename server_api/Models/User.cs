using System.ComponentModel.DataAnnotations;

namespace server_api.Models
{
    public class User
    {
        
       public enum Role
        {
            User,
            Admin
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string FirstName { get; set; }=string.Empty;
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string LastName { get; set; }= string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string Phone { get; set; }=string.Empty ;

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Password { get; set; }
        [Required]
        public Role RoleUser { get; set; }= Role.User;
    }
}
