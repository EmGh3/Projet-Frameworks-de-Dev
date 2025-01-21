namespace ERP_Project.Services.Contracts
{
    public interface INotificationService
    {
        Task<List<string>> GetManagerNotificationsAsync(string Id);
    }
}
