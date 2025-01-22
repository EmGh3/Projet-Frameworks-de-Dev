using Microsoft.AspNetCore.Mvc;


using ERP_Project.Services.Contracts;
using ERP_Project.Services.Services;
using ERP_Project.Models.viewModels;
using Microsoft.AspNetCore.Identity;
using ERP_Project.Models;
using Microsoft.AspNetCore.Authorization;
namespace ERP_Project.Controllers
{
    [Authorize]
    public class EmployeesListController : Controller
    {
        readonly IEmployeeService _employeeService;
        readonly EmailService _emailService;
        private readonly ILogger<EmployeesListController> _logger;
        private readonly UserManager<User> _userManager;


        public EmployeesListController(IEmployeeService employeeService ,EmailService emailService, ILogger<EmployeesListController> logger,UserManager<User> userManager)

        {
            _employeeService = employeeService;
            _emailService = emailService;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()

        {
            return View();
        }

        [Authorize(Roles = "ProjectManager")]
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

        [Authorize(Roles = "ProjectManager")]

        public async Task<IActionResult> SendEmailToEmployee(string  employeeId)
        {
            // Fetch the employee details by Id
            var employee =await _employeeService.GetById(employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            // Get the logged-in user's email (manager's email)

            // Pass employee details and manager email to the view
            var model = new SendEmailViewModel
            {
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                EmployeeEmail = employee.Email,
               
            };

            return View(model); // Return the email composition form view
        }


        [Authorize(Roles = "ProjectManager")]

        [HttpPost]
        public async Task<IActionResult> SendEmailToEmployee(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Use the manager's email (from ViewModel) and a dynamic name
                     var user = await _userManager.GetUserAsync(User);

                    var fromName = user.FirstName + " " + user.LastName;  // Dynamically get the logged-in user's email
                    var fromEmail = user.Email; ;  // The manager's email (from the ViewModel)
                                                         // You can dynamically set this as needed (e.g., get from the logged-in user's profile)

                    await _emailService.SendEmailAsync(
                        model.EmployeeEmail,   // To Email (Employee's Email)
                        model.Subject,         // Subject
                        model.Body,            // Body
                        model.Signature,       // Signature
                        fromEmail,             // Dynamic 'From Email'
                        fromName               // Dynamic 'From Name'
                    );

                    TempData["SuccessMessage"] = "Email sent successfully!";
                    return RedirectToAction("ListEmployees", "EmployeesList");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while sending the email: {ex.Message}";
                    return View(model);
                }
            }

            return View(model);
        }

    }
}
