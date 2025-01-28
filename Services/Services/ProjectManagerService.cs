using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
namespace ERP_Project.Services.Services
{
    public class ProjectManagerService :GenericService<ProjectManager>, IProjectManagerService
    {
        private readonly IProjectManagerRepository _projectManagerRepository;

        public ProjectManagerService(IProjectManagerRepository projectManagerRepository) : base(projectManagerRepository)
        {
            _projectManagerRepository = projectManagerRepository;
        }

        public async Task<ProjectManager> GetByIdAsync(string id)
        {
            return await _projectManagerRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Project>> GetProjectsByManager(string id) {
            return await _projectManagerRepository.GetProjectsByManager(id);

        }
        public int GetProjectCountForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("UserId cannot be null or empty", nameof(userId));

            return _projectManagerRepository.CountProjectsForUser(userId);
        }
        public async Task<int> GetCompletedProjectsCountForUserAsync(string userId)
        {
            return await _projectManagerRepository.GetCompletedProjectsCountAsync(userId);
        }

    }
}
