using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Contracts;

namespace ERP_Project.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        // Constructor to inject the IEmployeeRepository dependency
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Get all employees with their associated department
        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDepartment()
        {
            return await _employeeRepository.GetAllEmployeesWithDepartment();
        }

        // Get the total count of employees
        public async Task<int> EmployeeCount()
        {
            return await _employeeRepository.EmployeeCount();
        }

        // Get all tasks assigned to a specific employee by their ID
        public async Task<IEnumerable<ProjectTask>> GetEmployeeTasksAsync(string employeeId)
        {
            return await _employeeRepository.GetEmployeeTasksAsync(employeeId);
        }

        // Get all projects assigned to a specific employee by their ID
        public async Task<IEnumerable<Project>> GetEmployeeProjects(string employeeId)
        {
            return await _employeeRepository.GetEmployeeProjects(employeeId);
        }

        // Get an employee by their ID
        public async Task<Employee> GetById(string id)
        {
            return await _employeeRepository.GetById(id);
        }
    }
}
