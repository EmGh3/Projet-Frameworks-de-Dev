using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Contracts;

namespace ERP_Project.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IProjectManagerRepository _projectManagerRepository;

        public NotificationService(IProjectManagerRepository projectManagerRepository)
        {
            _projectManagerRepository = projectManagerRepository;
        }

        public async Task<List<string>> GetManagerNotificationsAsync(String Id)
        {
            var notifications = new List<string>();

            var projects = await _projectManagerRepository.GetProjectsByManager(Id);

            foreach (var project in projects)
            {
                if (project.EndDate!=null)
                {
                    var daysRemaining = (project.EndDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;

                    if (daysRemaining <= 7 && daysRemaining >= 0) 
                    {
                        notifications.Add($"Project '{project.Name}' deadline is in {daysRemaining} days!");
                    }
                }
            }

            return notifications;
        }

       
    }
}
