using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace web_final_project.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(60)]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        // Customer properties
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }

        // Admin flag
        public bool IsAdmin { get; set; } = false;

        public Cart? Cart { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}



