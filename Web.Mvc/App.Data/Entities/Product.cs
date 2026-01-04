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
        public string Title { get; set; } = "";
        public decimal Price { get; set; }

        public int CategoryId { get; set; }      
        public Category? Category { get; set; }  
    }
}
