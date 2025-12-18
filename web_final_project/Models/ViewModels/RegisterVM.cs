using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models.ViewModels
{
    public class RegisterVM
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        public string? Address { get; set; }
        public string? Phone { get; set; }
    }

}

