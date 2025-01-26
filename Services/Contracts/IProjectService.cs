using ERP_Project.Models;
using System.Collections;

namespace ERP_Project.Services.Contracts
{
    public interface IProjectService : IGenericService<Project>
    {
        Task<IEnumerable<Project>> GetProjectsWithDetailsAsync();
        Task<IEnumerable<Project>> GetProjectsByManagerAsync(string projectManagerId); // Add this method to the interface
        Task<int> GetDelayedProjectsCountForUserAsync(string userId);
        Task<Project> GetProjectWithTasksAsync(int projectId);

    }
}
