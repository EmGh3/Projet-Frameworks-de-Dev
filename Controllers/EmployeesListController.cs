using Microsoft.AspNetCore.Mvc;


using ERP_Project.Services.Contracts;
namespace ERP_Project.Controllers
{
    public class EmployeesListController : Controller
    {
        readonly IEmployeeService _employeeService;
        public EmployeesListController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index()

        {
            return View();
        }

        public async Task<IActionResult> ListEmployees()
        {
            // Await the async call to get employees
            var employees = await _employeeService.GetAllEmployeesWithDepartment();

            // Get the employee count (you can also await if it's an async method)
            var count = await _employeeService.EmployeeCount();

            // Pass the employee count to the view
            ViewBag.EmployeeCount = count;

            // Return the view with the employees
            return View(employees);
        }
    }
}
