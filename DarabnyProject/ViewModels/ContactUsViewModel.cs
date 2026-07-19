using System.ComponentModel.DataAnnotations;

namespace DarabnyProject.ViewModels
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";

        [MaxLength(200)]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [MinLength(10, ErrorMessage = "Message must be at least 10 characters")]
        [MaxLength(2000)]
        public string Message { get; set; } = "";
    }
}
