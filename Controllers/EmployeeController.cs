using ERP_Project.Models.viewModels;
using ERP_Project.Repositories;
using ERP_Project.Services.Contracts;
using Microsoft.AspNetCore.Mvc;


namespace ERP_Project.Controllers
{
    public class EmployeeController : Controller  // Inherit from Controller to handle actions
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        { 
            _employeeService = employeeService;
        }
            
        

        // Corrected method with proper type for 'id' (assuming it's an integer)
        public async Task<IActionResult> EmployeeProfile(string id)
        {
            var employee = await _employeeService.GetById(id);  // Await the async call to get the employee
            if (employee == null)  // Check if employee exists
            {
                return NotFound();  // Return a 404 if not found
            }
            var projects = await _employeeService.GetEmployeeProjects(id);
            var tasks = await _employeeService.GetEmployeeTasksAsync(id);
            var viewModel = new EmployeeProfileViewModel
            {
                Employee = employee,
                Projects = projects.ToList(),
                Tasks = tasks.ToList()
            };

            return View(viewModel); // Pass the ViewModel to the view
        }
    }

    }
