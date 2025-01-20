using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Contracts;
namespace ERP_Project.Services.Services
{
    public class ProjectManagerService : IProjectManagerService
    {
        private readonly IProjectManagerRepository _projectManagerRepository;

        // Constructor to inject the IProjectManagerRepository dependency
        public ProjectManagerService(IProjectManagerRepository projectManagerRepository)
        {
            _projectManagerRepository = projectManagerRepository;
        }

        // Get a project manager by their ID
        public async Task<ProjectManager> GetByIdAsync(string id)
        {
            return await _projectManagerRepository.GetByIdAsync(id);
        }
    }
}
