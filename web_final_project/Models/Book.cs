using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace web_final_project.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }


        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;


        [Required, MaxLength(120)]
        public string Author { get; set; } = null!;


        public string? Description { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }


        public int Stock { get; set; }

  
        public string? CoverImage { get; set; }


        public int? CategoryId { get; set; }
        public Category? Category { get; set; }


        public ICollection<CartItem>? CartItems { get; set; }
    }
}

