using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface IEmployeeService : IGenericService<Employee>
    {
        Task<IEnumerable<Employee>> GetAllEmployeesWithDepartment();
        Task<int> EmployeeCount();
        Task<IEnumerable<ProjectTask>> GetEmployeeTasksAsync(string employeeId);
        Task<IEnumerable<Project>> GetEmployeeProjects(string employeeId);
        Task<Employee> GetById(string id);
        Task<int> GetProjectCountForEmployeeAsync(string employeeId);
        Task<int> GetCompletedProjectsCountForEmployeeAsync(string employeeId);
        Task<int> GetDelayedProjectsCountForEmployeeAsync(string employeeId);
    }
}
