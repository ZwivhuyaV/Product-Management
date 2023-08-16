using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.NotificationRepository
{
    public interface INotificationRepository
    {
        Task DeleteNotification(Notification notification);
        Notification GetNotificationByDescription(string description);
        Task<Notification> GetNotificationById(int notificationId);
        Notification GetNotificationByProductId(int productiId);
        Task<List<Notification>> GetNotifications();
        Task SaveNotification(Notification notification);
    }
}
