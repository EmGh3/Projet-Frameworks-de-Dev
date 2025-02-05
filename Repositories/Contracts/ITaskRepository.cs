using ERP_Project.Models;

namespace ERP_Project.Repositories.Contracts
{
    public interface ITaskRepository : IGenericRepository<ProjectTask>
    {
        public Task<ProjectTask> GetByIdWithIncludes(int id);
        public Task<IEnumerable<ProjectTask>> GetByProjectId(int projectId);
        public Task<IEnumerable<ProjectTask>> GetByEmployeeId(string employeeId);
        public Task<IEnumerable<ProjectTask>> GetByProjectManagerId(string projectManagerId);
    }
}
