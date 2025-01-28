using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface IProjectManagerService :IGenericService<ProjectManager>
    {

      
            Task<ProjectManager> GetByIdAsync(string id);
        Task<IEnumerable<Project>> GetProjectsByManager(string id);

        int GetProjectCountForUser(string userId);

        Task<int> GetCompletedProjectsCountForUserAsync(string userId);

    }
}
