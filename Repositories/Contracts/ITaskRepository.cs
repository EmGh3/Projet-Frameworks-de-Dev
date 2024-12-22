using ERP_Project.Models;

namespace ERP_Project.Repositories.Contracts
{
    public interface ITaskRepository : IGenericRepository<ProjectTask>
    {
        public ProjectTask GetByIdWithIncludes(int id);
        public IEnumerable<ProjectTask> GetByProjectId(int projectId);
    }
}
