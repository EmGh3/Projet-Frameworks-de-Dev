using ERP_Project.Dtos;

namespace ERP_Project.Services.Contracts
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetManagerNotificationsAsync(string Id);
    }
}
