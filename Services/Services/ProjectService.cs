using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Services;
using NuGet.Protocol.Core.Types;

namespace ERP_Project.Services.Contracts
{
    public class ProjectService : GenericService<Project>, IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository) : base(projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetProjectsWithDetailsAsync()
        {
            return await _projectRepository.GetProjectsWithDetailsAsync();
        }
      
        public Task<IEnumerable<Project>> GetProjectsByManagerAsync(string projectManagerId)
        {
            return _projectRepository.GetAllProjectsByManagerAsync(projectManagerId);
        }
        public async Task<int> GetDelayedProjectsCountForUserAsync(string userId)
        {
            return await _projectRepository.GetDelayedProjectsCountAsync(userId);
        }
        public async Task<Project> GetProjectWithTasksAsync(int projectId)
        {
            // Call the repository to fetch the project along with tasks
            return await _projectRepository.GetProjectWithTasksAsync(projectId);
        }
        public IEnumerable<ProjectTask> GetTasks(int projectId)
        {
            return ((IProjectRepository)_projectRepository).GetTasks(projectId);
        }
        public void UpdateProgress(int projectId)
        {
            var project = _projectRepository.GetByIdAsync(projectId).Result;
            var tasks = GetTasks(projectId);
            var totalTasks = tasks.Count();
            var finishedTasks = tasks.Where(t => t.Status == ProjectTaskStatus.Finished).Count();
            project.Progress = (finishedTasks * 100) / totalTasks;
            _projectRepository.UpdateAsync(project).Wait();
        }
        public void UpdateExpenses(int projectId)
        {
            var project = _projectRepository.GetByIdAsync(projectId).Result;
            var tasks = GetTasks(projectId);
            var totalExpense = tasks.Sum(t => t.Cost);
            project.Expenses = (decimal)totalExpense;
            _projectRepository.UpdateAsync(project).Wait();
        }
        public IEnumerable<Project> GetByProjectManagerId(string projectManagerId)
        {
            return ((IProjectRepository)_projectRepository).GetByProjectManagerId(projectManagerId);
        }
        public void AddEmployeeToProject(int projectId, Employee employee)
        {
            ((IProjectRepository)_projectRepository).AddEmployeeToProject(projectId, employee);
        }
    }
}
