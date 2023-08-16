using ProductManagement.DAL.Models;
using System;

namespace ProductManagement.DAL.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public ProductDto(int productId, string productName, string productDescription, 
                          string productCode, int quantity, 
                          decimal price, decimal discountedPrice, DateTime expiryDate, 
                          bool isActive, DateTime createdDate, DateTime? updatedDate,
                          int employeeId)
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductCode = productCode;
            Quantity = quantity;
            Price = price;
            DiscountedPrice = discountedPrice;
            ExpiryDate = expiryDate;
            IsActive = isActive;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            EmployeeId = employeeId;
        }
    }
}
