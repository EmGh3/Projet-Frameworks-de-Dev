using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface ITaskService : IGenericService<ProjectTask>
    {
        public Task<ProjectTask> GetByIdWithIncludes(int id);
        public Task<IEnumerable<ProjectTask>> GetByProjectId(int projectId);
        public Task ChangeStatus(ProjectTask projectTask, ProjectTaskStatus status);
        public Task AssignTask(ProjectTask projectTask, Employee employee);
        public Task RemoveEmployee(ProjectTask projectTask);
        public Task<IEnumerable<ProjectTask>> GetByEmployeeId(string employeeId);
        public Task<IEnumerable<ProjectTask>> GetByProjectManagerId(string projectManagerId);
    }
}
