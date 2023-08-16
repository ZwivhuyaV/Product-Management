using Microsoft.EntityFrameworkCore;
using ProductManagement.DAL.DataContext;
using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.NotificationRepository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ProductManagementDBContext dbcontext;
        public NotificationRepository(ProductManagementDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task SaveNotification(Notification notification)
        {
            try
            {
                var savedRecord = dbcontext.Add(notification);
                await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteNotification(Notification notification)
        {
            try
            {
                dbcontext.Update(notification);
                await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Notification> GetNotificationById(int notificationId)
        {
            try
            {
                return await dbcontext.FindAsync<Notification>(notificationId);
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
                var notification = dbcontext.Notification.Where(n => n.ProductId == productiId && n.IsActive).FirstOrDefault();
                return notification;
            }
            catch
            {
                throw;
            }
        }

        public Notification GetNotificationByDescription(string description)
        {
            try
            {
                return dbcontext.Set<Notification>().Where(n => n.NotificationDescription.Equals(description, StringComparison.OrdinalIgnoreCase) && n.IsActive).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Notification>> GetNotifications()
        {
            try
            {
                return await dbcontext.Set<Notification>().Where(n => n.IsActive).ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
