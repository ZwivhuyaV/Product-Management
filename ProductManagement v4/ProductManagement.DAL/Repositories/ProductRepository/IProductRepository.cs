using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<int> DeleteProduct(Product product);
        Task<List<Product>> GetInActiveProducts();
        Product GetProductByCode(string code);
        Product GetProductByCodeOrName(string code, string name);
        Task<Product> GetProductById(int productId);
        Task<List<Product>> GetActiveProducts();
        Product GetProductsByName(string name);
        Task<int> SaveProduct(Product product);
        Task<List<Product>> SearchInActiveProduct(string searchField);
        Task<List<Product>> SearchActiveProduct(string searchField);
        Task<int> UpdateProduct(Product product);
    }
}
