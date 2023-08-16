using ProductManagement.DAL.DataContext;
using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.AuditRepository
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ProductManagementDBContext dbcontext;
        public AuditRepository(ProductManagementDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task SaveAudit(AuditEvent audit)
        {
            try
            {
                var savedRecord = dbcontext.Add(audit);
                await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
