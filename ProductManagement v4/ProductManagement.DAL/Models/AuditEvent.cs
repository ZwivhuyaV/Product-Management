using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Models
{
    public class AuditEvent : CommonEntity
    {
        [Key]
        public int AuditEventId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Payload { get; set; }
    }
}
