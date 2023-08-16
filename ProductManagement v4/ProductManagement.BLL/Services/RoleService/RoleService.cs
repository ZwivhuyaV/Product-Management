using ProductManagement.DAL.Models;
using ProductManagement.DAL.Repositories.RoleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }
        public async Task<List<Role>> GetRoles()
        {
            try
            {
                return await roleRepository.GetRoles();
            }
            catch
            {
                throw;
            }
        }
    }
}
