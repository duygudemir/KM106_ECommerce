using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }              
        public string Name { get; set; } = "";

        public string Color { get; set; } = "";
        public string IconCssClass { get; set; } = "";
        public DateTime CreatedAt { get; set; }

    }
}
