using System.ComponentModel.DataAnnotations;

namespace ProductManagement.DAL.Models
{
    public class Notification : CommonEntity
    {
        [Key]
        public int NotificationId { get; set; }
        public string NotificationDescription { get; set; }
        public int ProductId { get; set; }
    }
}
