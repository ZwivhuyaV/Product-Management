﻿using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.AuditRepository
{
    public interface IAuditRepository
    {
        Task SaveAudit(AuditEvent audit);
    }
}
