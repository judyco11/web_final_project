using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Author { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public string? Edition { get; set; }
        public string? CoverImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int PopularityScore { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
    }
}



