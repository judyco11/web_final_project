using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Customer"; // "Customer" or "Admin"

        [MaxLength(250)]
        public string? Address { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? Phone { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<Reviews>? Reviews { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
