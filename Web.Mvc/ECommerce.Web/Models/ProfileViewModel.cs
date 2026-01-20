using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models
{
    public class ProfileViewModel
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";
    }
}
