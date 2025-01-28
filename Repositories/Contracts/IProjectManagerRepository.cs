using ERP_Project.Models;

namespace ERP_Project.Repositories.Contracts
{
    public interface IProjectManagerRepository: IGenericRepository<ProjectManager>
    {


        Task<ProjectManager> GetByIdAsync(string id);
        Task<IEnumerable<Project>> GetProjectsByManager(string id);
        Task<int> GetCompletedProjectsCountAsync(string userId);
        int CountProjectsForUser(string userId);
    }
}
