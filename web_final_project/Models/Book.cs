

namespace OnlineBookStors.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string? Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal? Price { get; set; }
        public int Stock { get; set; }

        public string? CoverImage { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}



