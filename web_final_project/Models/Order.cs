namespace OnlineBookStors.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Shipped

        public int? TotalAmount { get; set; }
        public int UserId { get; set; }
        public User ? User { get; set; }

        public DateTime Created { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}


