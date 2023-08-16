using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using ProductManagement.BLL.Services.NotificationService;
using ProductManagement.BLL.Services.ProductService;
using ProductManagement.DAL.Common;
using ProductManagement.DAL.Constants;
using ProductManagement.DAL.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.WorkerJobService
{
    public class NotificationJob : IJob
    {
        private readonly IProductService productService;
        private readonly INotificationService notificationService;
        private readonly IOptions<AppSettings> settings;
        public NotificationJob(IProductService productService,
                               INotificationService notificationService,
                               IOptions<AppSettings> settings)
        {
            this.productService = productService;
            this.notificationService = notificationService;
            this.settings = settings;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var totalProducts = await productService.GetActiveProducts();

            foreach (var product in totalProducts)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract((DateTime)product.ExpiryDate);
                var totalWeeks = (int)timeSpan.Days / settings.Value.NumberOfDaysToNotify;

                if (totalWeeks >= ConstantsValues.TotalWeeksMoreThan)
                {
                    var notification = notificationService.GetNotificationByProductId(product.ProductId);
                    if (notification == null)
                    {
                        var newNotification = new Notification()
                        {
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                            ProductId = product.ProductId,
                            NotificationDescription = string.Format(ConstantsValues.ProductExpiringSoon, product.ProductName, product.ProductCode, product.ExpiryDate.ToShortDateString()),
                        };
                        await notificationService.CreateNotification(newNotification);
                    }
                }
            }
        }
    }
}
