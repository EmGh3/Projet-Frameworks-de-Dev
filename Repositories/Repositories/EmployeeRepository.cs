using ERP_Project.Data;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
namespace ERP_Project.Repositories.Repositories
{
    public class EmployeesRepository:GenericRepository<Employee>, IEmployeeRepository
    {


        private readonly ApplicationDbContext _context;

        public EmployeesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;


        }
        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDepartment()
        {
            return await _context.Employees
                         .Include(e => e.Department)
                         .ToListAsync();
        }


        public async Task<Employee> GetById(string id)
        {
            return await _context.Employees
                         .Include(e => e.Department)
                         .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<int> EmployeeCount()
        {
            return await _context.Employees.CountAsync();
        }


        public async Task<IEnumerable<Project>> GetEmployeeProjects(string employeeId)
        {
            return await _context.Projects
                                 .Where(p => p.Employees.Any(e => e.Id == employeeId)) // Check if employee is part of the project
                                 .Include(p => p.Employees)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetEmployeeTasksAsync(string id)
        {
           
            return await _context.Tasks
                                 .Where(t => t.EmployeeId == id)
                                 .ToListAsync();
        }

       
    }
}
