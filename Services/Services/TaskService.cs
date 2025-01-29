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
        public IEnumerable<ProjectTask> GetByEmployeeId(string employeeId)
        {
            return ((ITaskRepository)_repository).GetByEmployeeId(employeeId);
        }
        public IEnumerable<ProjectTask> GetByProjectManagerId(string projectManagerId)
        {
            return ((ITaskRepository)_repository).GetByProjectManagerId(projectManagerId);
        }
    }

}
