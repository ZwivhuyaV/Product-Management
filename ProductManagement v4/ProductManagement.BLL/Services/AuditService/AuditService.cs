using Microsoft.EntityFrameworkCore;
using ProductManagement.DAL.Models;
using ProductManagement.DAL.Repositories.AuditRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.AuditService
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository auditRepository;
        public AuditService(IAuditRepository auditRepository)
        {
            this.auditRepository = auditRepository;
        }

        public async Task CreateAudit(AuditEvent audit)
        {
            try
            {
                await auditRepository.SaveAudit(audit);
            }
            catch
            {
                throw;
            }
        }
    }
}
