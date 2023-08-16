using Microsoft.EntityFrameworkCore;
using ProductManagement.DAL.DataContext;
using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductManagementDBContext dbcontext;
        public ProductRepository(ProductManagementDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<List<Product>> GetActiveProducts()
        {
            try
            {
                return await dbcontext.Set<Product>().Where(p => p.IsActive).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Product>> GetInActiveProducts()
        {
            try
            {
                return await dbcontext.Set<Product>().Where(p => !p.IsActive).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Product>> SearchActiveProduct(string searchField)
        {
            try
            {
                return await dbcontext.Set<Product>().Where(p => p.IsActive &&
                                                                  (p.ProductCode.Contains(searchField) || 
                                                                  p.ProductName.Contains(searchField) ||
                                                                  p.ProductDescription.Contains(searchField))).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Product>> SearchInActiveProduct(string searchField)
        {
            try
            {
                return await dbcontext.Set<Product>().Where(p => !p.IsActive &&
                                                                  (p.ProductCode.Contains(searchField) ||
                                                                  p.ProductName.Contains(searchField) ||
                                                                  p.ProductDescription.Contains(searchField))).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public Product GetProductsByName(string name)
        {
            try
            {
                return dbcontext.Set<Product>().Where(p => p.IsActive && p.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public Product GetProductByCode(string code)
        {
            try
            {
                return dbcontext.Set<Product>().Where(p => p.IsActive && p.ProductCode.Equals(code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public Product GetProductByCodeOrName(string code, string name)
        {
            try
            {
                return dbcontext.Set<Product>().Where(p => p.IsActive && p.ProductCode == code || p.ProductName == name).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                return await dbcontext.Set<Product>().Where(p => p.IsActive && p.ProductId == productId).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> SaveProduct(Product product)
        {
            try
            {
                var savedRecord = dbcontext.Add(product);
                return await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpdateProduct(Product product)
        {
            try
            {
                dbcontext.Update(product);
                return await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> DeleteProduct(Product product)
        {
            try
            {
                dbcontext.Update(product);
                return await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
