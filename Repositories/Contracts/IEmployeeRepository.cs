using ERP_Project.Models;
using System.Threading.Tasks;

namespace ERP_Project.Repositories.Contracts
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {

        Task<IEnumerable<Employee>> GetAllEmployeesWithDepartment();
        Task<int> EmployeeCount();
        Task<IEnumerable<ProjectTask>> GetEmployeeTasksAsync(string id);
        Task<IEnumerable<Project>> GetEmployeeProjects(string employeeId);
        Task<Employee> GetById(string id);
        Task<IEnumerable<Employee>> GetByProjectId(int id);

    }
}
