using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Contracts;

namespace ERP_Project.Services.Services
{
    public class TaskService: GenericService<ProjectTask>, ITaskService
    {
        public TaskService(ITaskRepository repository) : base(repository)
        {
            
        }
        public ProjectTask GetByIdWithIncludes(int id)
        {
            return ((ITaskRepository)_repository).GetByIdWithIncludes(id);
        }
        public IEnumerable<ProjectTask> GetByProjectId(int projectId)
        {
            return ((ITaskRepository)_repository).GetByProjectId(projectId);
        }
    }

}
