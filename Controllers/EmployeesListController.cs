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
            var employees = await _employeeService.GetAllEmployeesWithDepartment();

            var count = await _employeeService.EmployeeCount();

            ViewBag.EmployeeCount = count;

            return View(employees);
        }

        [Authorize(Roles = "ProjectManager")]

        public async Task<IActionResult> SendEmailToEmployee(string  employeeId)
        {
            var employee =await _employeeService.GetById(employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            
            var model = new SendEmailViewModel
            {
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                EmployeeEmail = employee.Email,
               
            };

            return View(model); 
        }


        [Authorize(Roles = "ProjectManager")]

        [HttpPost]
        public async Task<IActionResult> SendEmailToEmployee(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                     var user = await _userManager.GetUserAsync(User);

                    var fromName = user.FirstName + " " + user.LastName;  
                    var fromEmail = user.Email; ;  
                                                        

                    await _emailService.SendEmailAsync(
                        model.EmployeeEmail,   
                        model.Subject,         
                        model.Body,            
                        model.Signature,       
                        fromEmail,             
                        fromName               
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
