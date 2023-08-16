using Microsoft.EntityFrameworkCore;
using ProductManagement.DAL.Models;

namespace ProductManagement.DAL.DataContext
{
    public class ProductManagementDBContext : DbContext
    {
        public ProductManagementDBContext(DbContextOptions<ProductManagementDBContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<AuditEvent> AuditEvent { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Role> Role { get; set; }
    }
}
