using System.ComponentModel.DataAnnotations;

namespace Project_MVC.Models.Auth
{
    public class LoginModelRequest
    {
        [Required(ErrorMessage = "Username không được để trống")]
        public string Username { get; set; } 
        [Required(ErrorMessage = "Password không được để trống")]
        public string Password { get; set; } 

        public string? Erorr { get; set; }
    }
}
