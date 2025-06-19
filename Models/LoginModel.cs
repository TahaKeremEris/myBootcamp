using System.ComponentModel.DataAnnotations;

namespace BootcampDay1.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        public string? Password { get; set; }
    }
}

