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
        public IEnumerable<ProjectTask> GetTasks(int projectId);
        public void UpdateProgress(int projectId);
        public void UpdateExpenses(int projectId);
        public IEnumerable<Project> GetByProjectManagerId(string projectManagerId);
        public void AddEmployeeToProject(int ProjectId, Employee employee);

    }
}
