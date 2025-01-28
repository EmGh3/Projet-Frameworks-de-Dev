using Microsoft.AspNetCore.Mvc;
using ERP_Project.Services.Contracts;
namespace ERP_Project.Views.Shared.Components.Notifications
{
    [ViewComponent] 
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;

        public NotificationsViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string managerId)
        {
            var notifications = await _notificationService.GetManagerNotificationsAsync(managerId);
            return View(notifications);
        }

    }
} 

