using System;

namespace ProductManagement.Models
{
    public class NotificationsViewModel
    {
        public int NotificationId { get; set; }
        public string NotificationDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
