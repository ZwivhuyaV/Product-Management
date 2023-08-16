using Microsoft.EntityFrameworkCore;
using ProductManagement.DAL.Models;
using ProductManagement.DAL.Repositories.NotificationRepository;
using ProductManagement.DAL.Repositories.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task CreateNotification(Notification notification)
        {
            var existingRecord = notificationRepository.GetNotificationByProductId(notification.ProductId);

            //Record already exists
            if (existingRecord != null)
                return;

            await notificationRepository.SaveNotification(notification);
        }

        public async Task DeleteNotification(int notificationId)
        {
            var existingRecord = await notificationRepository.GetNotificationById(notificationId);

            //Record doesn't exist
            if (existingRecord == null)
                return;

            existingRecord.UpdatedDate = DateTime.Now;
            existingRecord.IsActive = false;

            await notificationRepository.DeleteNotification(existingRecord);
        }

        public async Task<List<Notification>> GetNotifications()
        {
            try
            {
                return await notificationRepository.GetNotifications();
            }
            catch
            {
                throw;
            }
        }

        public Notification GetNotificationByProductId(int productiId)
        {
            try
            {
                return notificationRepository.GetNotificationByProductId(productiId);
            }
            catch
            {
                throw;
            }
        }
    }
}
