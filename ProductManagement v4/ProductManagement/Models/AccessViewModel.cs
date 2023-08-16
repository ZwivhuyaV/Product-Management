using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class AccessViewModel
    {
        [Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password is required")]
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
