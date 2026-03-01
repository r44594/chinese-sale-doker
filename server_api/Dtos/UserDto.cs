using System.ComponentModel.DataAnnotations;

namespace server_api.Dtos
{
    public class UserDto
    {

        public class RegisterDto
        {
            [Required]
            [MaxLength(30)]
            [MinLength(4)]
            public string UserName { get; set; } = string.Empty;

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
            public string Email { get; set; } = string.Empty;

            [Required]
            [Phone]
            public string Phone { get; set; } = string.Empty;

            [Required]
            [MaxLength(30)]
            [MinLength(4)]
            public string Password { get; set; } = string.Empty;
        }


       

    }
    public class GetUsers
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = "User";

    }
    public class LoginDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Password { get; set; } = string.Empty;
    }
}
