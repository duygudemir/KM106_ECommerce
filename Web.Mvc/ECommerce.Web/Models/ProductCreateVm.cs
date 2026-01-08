using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class ProductCreateVm
    {
        [Required(ErrorMessage = "Kategori seçmek zorunludur.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün adı 2-100 karakter olmalı.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, 9999999, ErrorMessage = "Fiyat 0'dan büyük olmalı.")]
        public decimal Price { get; set; }

        [StringLength(1000, ErrorMessage = "Detay en fazla 1000 karakter olabilir.")]
        public string? Details { get; set; }

        [Required(ErrorMessage = "Stok zorunludur.")]
        [Range(1, 255, ErrorMessage = "Stok 1-255 arası olmalı.")]
        public int StockAmount { get; set; }
    }
}
