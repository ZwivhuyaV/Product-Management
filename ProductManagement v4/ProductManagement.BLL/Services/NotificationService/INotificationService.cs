using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.NotificationService
{
    public interface INotificationService
    {
        Task CreateNotification(Notification notification);
        Task DeleteNotification(int notificationId);
        Notification GetNotificationByProductId(int productiId);
        Task<List<Notification>> GetNotifications();
    }
}
