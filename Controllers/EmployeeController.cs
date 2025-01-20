using ERP_Project.Repositories;
using ERP_Project.Repositories.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Project.Controllers
{
    public class EmployeeController : Controller  // Inherit from Controller to handle actions
    {
        private readonly EmployeesRepository _employeeRepository;

        public EmployeeController(EmployeesRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Corrected method with proper type for 'id' (assuming it's an integer)
        public IActionResult EmployeeDetails(string id)  // Assuming 'id' is an integer
        {
            var employee = _employeeRepository.GetById(id);  // Get the employee by id
            if (employee == null)  // Check if employee exists
            {
                return NotFound();  // Return a 404 if not found
            }

            return View(employee);  // Pass the employee object to the view
        }
    }
}
