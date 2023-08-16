using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class PasswordViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 7, ErrorMessage = "New passwords must be a minimum of 7 characters, please try a different password.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The Confirmation password is required")]
        [StringLength(50, MinimumLength = 7, ErrorMessage = "New passwords must be a minimum of 7 characters, please try a different password.")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set;}
        [Required]
        public string CurrentPassword { get; set; }
    }
}
