using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class Cart : BaseEntity
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation-like property (not EF, just useful in code)
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
