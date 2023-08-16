using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.AuditService
{
    public interface IAuditService
    {
        Task CreateAudit(AuditEvent audit);
    }
}
