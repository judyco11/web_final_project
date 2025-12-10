using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace web_final_project.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }


        public int UserId { get; set; }
        public AppUser? User { get; set; }


        public ICollection<CartItem>? CartItems { get; set; }
    }
}
