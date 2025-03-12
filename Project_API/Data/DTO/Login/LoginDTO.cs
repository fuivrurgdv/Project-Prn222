using System.ComponentModel.DataAnnotations;

namespace Project_API.Data.DTO.Login
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; } // Email
        [Required]
        public string Password { get; set; }

    }
}
