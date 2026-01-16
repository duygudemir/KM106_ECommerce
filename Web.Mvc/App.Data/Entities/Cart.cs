using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public decimal TotalPrice { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public List<CartItem> CartItems { get; set; } = new();
    }
}
