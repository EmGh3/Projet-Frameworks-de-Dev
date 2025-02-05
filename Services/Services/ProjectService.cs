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
            return await _projectRepository.GetProjectWithTasksAsync(projectId);
        }
        public async Task<IEnumerable<ProjectTask>> GetTasks(int projectId)
        {
            return await _projectRepository.GetTasks(projectId);
        }
        public async Task UpdateProgress(int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            var tasks = await GetTasks(projectId);
            if (tasks == null)
            {
                return;
            }
            var totalTasks = tasks.Count();
            var finishedTasks = tasks.Where(t => t.Status == ProjectTaskStatus.Finished).Count();
            project.Progress = (finishedTasks * 100) / totalTasks;
            if(project.Progress == 100) {
                project.Status = "Completed";
            }
            else if (project.Progress > 0)
            {
                project.Status = "In Progress";
            }
            else
            {
                project.Status = "Not Started";
            }
            await _projectRepository.UpdateAsync(project);
        }
        public async Task UpdateExpenses(int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            var tasks = await GetTasks(projectId);
            if (tasks == null)
            {
                return;
            }
            var totalExpense = tasks.Sum(t => t.Cost);
            project.Expenses = (decimal)totalExpense;
            await _projectRepository.UpdateAsync(project);
        }
        public async Task<IEnumerable<Project>> GetByProjectManagerId(string projectManagerId)
        {
            return await _projectRepository.GetByProjectManagerId(projectManagerId);
        }
        public async Task AddEmployeeToProject(int projectId, Employee employee)
        {
            await _projectRepository.AddEmployeeToProject(projectId, employee);
        }
    }
}
