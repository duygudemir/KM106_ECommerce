using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class ProductEditStockVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Stok zorunludur.")]
        [Range(0, 255, ErrorMessage = "Stok 0-255 arası olmalı.")]
        public int StockAmount { get; set; }
    }
}
