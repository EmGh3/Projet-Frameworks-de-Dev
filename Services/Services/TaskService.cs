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
        public async Task<ProjectTask> GetByIdWithIncludes(int id)
        {
            return await ((ITaskRepository)_repository).GetByIdWithIncludes(id);
        }
        public async Task<IEnumerable<ProjectTask>> GetByProjectId(int projectId)
        {
            return await ((ITaskRepository)_repository).GetByProjectId(projectId);
        }
        public Task ChangeStatus(ProjectTask projectTask, ProjectTaskStatus status)
        {
            projectTask.Status = status;
            return UpdateAsync(projectTask);
        }
        public Task AssignTask(ProjectTask projectTask, Employee employee)
        {
            projectTask.Employee = employee;
            return UpdateAsync(projectTask);
        }
        public Task RemoveEmployee(ProjectTask projectTask)
        {
            projectTask.Employee = null;
            projectTask.EmployeeId = null;
            return UpdateAsync(projectTask);
        }
        public async Task<IEnumerable<ProjectTask>> GetByEmployeeId(string employeeId)
        {
            return await ((ITaskRepository)_repository).GetByEmployeeId(employeeId);
        }
        public async Task<IEnumerable<ProjectTask>> GetByProjectManagerId(string projectManagerId)
        {
            return await ((ITaskRepository)_repository).GetByProjectManagerId(projectManagerId);
        }
    }

}
