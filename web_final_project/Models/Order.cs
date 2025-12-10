using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace web_final_project.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }


        public int UserId { get; set; }
        public AppUser? User { get; set; }


        public DateTime OrderDate { get; set; } = DateTime.UtcNow;


        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }


        public string Status { get; set; } = "Pending";
    }
}

