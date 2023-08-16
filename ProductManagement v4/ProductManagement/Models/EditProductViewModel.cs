using System.ComponentModel.DataAnnotations;
using System;

namespace ProductManagement.Models
{
    public class EditProductViewModel
    {
        [Required]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "The Name is required")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "The Description is required")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "The Code is required")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "The Quantity is required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "The Price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "The Expiry Date is required")]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
