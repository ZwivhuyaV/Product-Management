using Microsoft.EntityFrameworkCore;
using ProductManagement.DAL.DataContext;
using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ProductManagementDBContext dbcontext;
        public RoleRepository(ProductManagementDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<List<Role>> GetRoles()
        {
            try
            {
                return await dbcontext.Set<Role>().Where(r => r.IsActive).ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
