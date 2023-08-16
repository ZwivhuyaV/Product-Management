using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductManagement.BLL.Services.NotificationService;
using ProductManagement.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly INotificationService notificationService;

        public HomeController(ILogger<HomeController> logger, INotificationService notificationService)
        {
            this.logger = logger;
            this.notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var notificationsViewModel = new List<NotificationsViewModel>();
            var notifications = await notificationService.GetNotifications();

            foreach (var notification in notifications)
            {
                var notificationModel = new NotificationsViewModel()
                {
                    NotificationId = notification.NotificationId,
                    NotificationDescription = notification.NotificationDescription,
                    CreatedDate = notification.CreatedDate,
                    IsActive = notification.IsActive,
                };
                notificationsViewModel.Add(notificationModel);
            }

            return View(notificationsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await notificationService.DeleteNotification(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}