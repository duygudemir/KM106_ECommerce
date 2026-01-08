using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class ProductEditPriceVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, 9999999, ErrorMessage = "Fiyat 0'dan büyük olmalı.")]
        public decimal Price { get; set; }
    }
}
