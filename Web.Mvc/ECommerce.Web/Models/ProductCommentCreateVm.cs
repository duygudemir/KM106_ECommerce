using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class ProductCommentCreateVm
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Text { get; set; } = "";

        [Range(1, 5)]
        public int StarCount { get; set; } = 5;
    }
}
