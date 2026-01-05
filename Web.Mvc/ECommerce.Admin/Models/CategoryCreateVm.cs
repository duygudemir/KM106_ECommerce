using System.ComponentModel.DataAnnotations;

namespace ECommerce.Admin.Models
{
    public class CategoryCreateVm
    {
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Color zorunludur.")]
        [RegularExpression("^[0-9A-Fa-f]{6}$", ErrorMessage = "Color 6 haneli hex olmalı. Örn: FF5733")]
        public string Color { get; set; } = "";

        [Required(ErrorMessage = "IconCssClass zorunludur.")]
        [StringLength(50, ErrorMessage = "IconCssClass en fazla 50 karakter olabilir.")]
        public string IconCssClass { get; set; } = "";
    }
}
