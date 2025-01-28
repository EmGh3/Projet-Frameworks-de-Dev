using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface ITaskService : IGenericService<ProjectTask>
    {
        public ProjectTask GetByIdWithIncludes(int id);
        public IEnumerable<ProjectTask> GetByProjectId(int projectId);
        public Task ChangeStatus(ProjectTask projectTask, ProjectTaskStatus status);
        public Task AssignTask(ProjectTask projectTask, Employee employee);
        public Task RemoveEmployee(ProjectTask projectTask);
        public IEnumerable<ProjectTask> GetByEmployeeId(string employeeId);
        public IEnumerable<ProjectTask> GetByProjectManagerId(string projectManagerId);
    }
}
