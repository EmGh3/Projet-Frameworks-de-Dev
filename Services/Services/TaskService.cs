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
        // Get a task by its ID with all its includes
        public ProjectTask GetByIdWithIncludes(int id)
        {
            return ((ITaskRepository)_repository).GetByIdWithIncludes(id);
        }
        // Get all tasks assigned to a specific project by the project ID
        public IEnumerable<ProjectTask> GetByProjectId(int projectId)
        {
            return ((ITaskRepository)_repository).GetByProjectId(projectId);
        }
        // Change the status of a task
        public Task ChangeStatus(ProjectTask projectTask, ProjectTaskStatus status)
        {
            projectTask.Status = status;
            return UpdateAsync(projectTask);
        }
        // Assign a task to an employee
        public Task AssignTask(ProjectTask projectTask, Employee employee)
        {
            projectTask.Employee = employee;
            return UpdateAsync(projectTask);
        }
        // Remove an employee from a task
        public Task RemoveEmployee(ProjectTask projectTask)
        {
            projectTask.Employee = null;
            return UpdateAsync(projectTask);
        }
        // Get all tasks assigned to a specific employee by their ID
        public IEnumerable<ProjectTask> GetByEmployeeId(string employeeId)
        {
            return ((ITaskRepository)_repository).GetByEmployeeId(employeeId);
        }
        // Get all tasks assigned to a specific project manager by their ID
        public IEnumerable<ProjectTask> GetByProjectManagerId(string projectManagerId)
        {
            return ((ITaskRepository)_repository).GetByProjectManagerId(projectManagerId);
        }
    }

}
