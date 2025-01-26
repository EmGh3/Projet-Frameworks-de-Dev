using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Services;

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
    }
}
