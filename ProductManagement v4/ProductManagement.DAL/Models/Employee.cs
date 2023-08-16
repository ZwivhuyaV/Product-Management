using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.DAL.Models
{
    public class Employee : CommonEntity
    {
        [Key]
        public int EmployeeId { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [ForeignKey("EmployeeId")]
        public ICollection<Product> products { get; set; }
        public int RoleId { get; set; }
    }
}
