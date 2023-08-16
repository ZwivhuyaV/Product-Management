using System;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class AddEmployeeViewModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "The First Name is required")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "The Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
