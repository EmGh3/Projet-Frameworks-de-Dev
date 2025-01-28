using ERP_Project.Models;

namespace ERP_Project.Repositories.Contracts
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetProjectsWithDetailsAsync();
        Task<IEnumerable<Project>> GetAllProjectsByManagerAsync(string projectManagerId);
        Task<int> GetDelayedProjectsCountAsync(string userId);
        Task<Project> GetProjectWithTasksAsync(int projectId);
        public void AddEmployeeToProject(int ProjectId,Employee employee);
        public IEnumerable<ProjectTask> GetTasks(int projectId);
        public IEnumerable<Project> GetByProjectManagerId(string projectManagerId);
        
    }
}
