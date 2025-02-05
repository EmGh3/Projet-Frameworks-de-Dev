using ERP_Project.Models;
using System.Collections;

namespace ERP_Project.Services.Contracts
{
    public interface IProjectService : IGenericService<Project>
    {
        Task<IEnumerable<Project>> GetProjectsWithDetailsAsync();
        Task<IEnumerable<Project>> GetProjectsByManagerAsync(string projectManagerId); 
        Task<int> GetDelayedProjectsCountForUserAsync(string userId);
        Task<Project> GetProjectWithTasksAsync(int projectId);
        public Task<IEnumerable<ProjectTask>> GetTasks(int projectId);
        public Task UpdateProgress(int projectId);
        public Task UpdateExpenses(int projectId);
        public Task<IEnumerable<Project>> GetByProjectManagerId(string projectManagerId);
        public Task AddEmployeeToProject(int ProjectId, Employee employee);

    }
}
