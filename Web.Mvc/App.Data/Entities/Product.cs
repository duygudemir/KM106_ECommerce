using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public int SellerId { get; set; }
        public User? Seller { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string Name { get; set; } = "";      
        public decimal Price { get; set; }          
        public string? Details { get; set; }        
        public byte StockAmount { get; set; }       

        public DateTime CreatedAt { get; set; }     
        public bool Enabled { get; set; } = true;   
    }
}
