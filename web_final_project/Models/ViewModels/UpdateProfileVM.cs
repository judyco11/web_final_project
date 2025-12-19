using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models.ViewModels
{
    public class UpdateProfileVM
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MinLength(6)]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? Phone { get; set; }
    }
}
