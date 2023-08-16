using ProductManagement.DAL.DTOs;
using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.ProductService
{
    public interface IProductService
    {
        Task<ResponseMessage> CreateProduct(Product product);
        Task<ResponseMessage> DeleteProduct(int productId);
        ProductDto GetProductByCode(string code);
        Task<Product> GetProductById(int productId);
        ProductDto GetProductByName(string name);
        Task<List<ProductDto>> GetActiveProducts();
        Task<List<ProductDto>> SearchInActiveProducts(string searchField);
        Task<List<ProductDto>> SearchActiveProducts(string searchField);
        Task<ResponseMessage> UpdateProduct(Product product);
    }
}
