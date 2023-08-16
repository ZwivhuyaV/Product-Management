using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.DAL.Models
{
    public class Role : CommonEntity
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        [ForeignKey("RoleId")]
        public ICollection<Employee> employees { get; set; }
    }
}
