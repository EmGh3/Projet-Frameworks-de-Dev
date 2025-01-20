using Microsoft.AspNetCore.Mvc;


using ERP_Project.Repositories.Contracts;
namespace ERP_Project.Controllers
{
    public class EmployeesListController : Controller
    {
        readonly IEmployeeRepository _employeeRepository;
        public EmployeesListController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }               
        public IActionResult Index()

        {
            return View();
        }

        public async Task<IActionResult> ListEmployees()
        {
            // Await the async call to get employees
            var employees = await _employeeRepository.GetAllEmployeesWithDepartment();

            // Get the employee count (you can also await if it's an async method)
            var count = await _employeeRepository.EmployeeCount();

            // Pass the employee count to the view
            ViewBag.EmployeeCount = count;

            // Return the view with the employees
            return View(employees);
        }
    }
}
