using ERP_Project.Controllers;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Contracts;

namespace ERP_Project.Services.Services
{
    public class EmployeeService :GenericService<Employee> , IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

     
        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDepartment()
        {
            return await _employeeRepository.GetAllEmployeesWithDepartment();
        }

        public async Task<int> EmployeeCount()
        {
            return await _employeeRepository.EmployeeCount();
        }

        public async Task<IEnumerable<ProjectTask>> GetEmployeeTasksAsync(string employeeId)
        {
            return await _employeeRepository.GetEmployeeTasksAsync(employeeId);
        }

        public async Task<IEnumerable<Project>> GetEmployeeProjects(string employeeId)
        {
            return await _employeeRepository.GetEmployeeProjects(employeeId);
        }

        public async Task<Employee> GetById(string id)
        {
            return await _employeeRepository.GetById(id);
        }
        public async Task<IEnumerable<Employee>> GetByProjectId(int id)
        {
            return await _employeeRepository.GetByProjectId(id);
        }
        public async Task<int> GetProjectCountForEmployeeAsync(string employeeId)
        {
            return await _employeeRepository.GetProjectCountForEmployeeAsync(employeeId);
        }

        public async Task<int> GetCompletedProjectsCountForEmployeeAsync(string employeeId)
        {
            return await _employeeRepository.GetCompletedProjectsCountForEmployeeAsync(employeeId);
        }

        public async Task<int> GetDelayedProjectsCountForEmployeeAsync(string employeeId)
        {
            return await _employeeRepository.GetDelayedProjectsCountForEmployeeAsync(employeeId);
        }
    }
}
