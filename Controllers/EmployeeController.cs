using ERP_Project.Models;
using ERP_Project.Models.viewModels;
using ERP_Project.Repositories;
using ERP_Project.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ERP_Project.Controllers
{
    [Authorize]

    public class EmployeeController : Controller  // Inherit from Controller to handle actions
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeeController(IEmployeeService employeeService ,IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming user ID is stored in claims
            ViewBag.CurrentUserId = currentUserId;
            return View(viewModel); // Pass the ViewModel to the view
        }


        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(string id)
        {
            var employee = await _employeeService.GetById(id);    
            if (employee == null)
            {
                return NotFound();
            }

            var model = new UpdateEmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DepartmentId = employee.DepartmentId,
                Designation = employee.Designation
            };
            var departments = await _departmentService.GetAllAsync();
            ViewData["Departments"] = new SelectList(departments, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async  Task<IActionResult> UpdateEmployee(UpdateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {

                var departments = await _departmentService.GetAllAsync();
                ViewData["Departments"] = new SelectList(departments, "Id", "Name");
                return View(model);
            }

            var employee =await  _employeeService.GetById(model.Id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.PhoneNumber = model.PhoneNumber;
            employee.DepartmentId = model.DepartmentId;
            employee.Designation = model.Designation;

            await _employeeService.UpdateAsync(employee);

            return RedirectToAction("EmployeeProfile", new { id = model.Id });
        }


        public async Task<IActionResult> EmployeeCalendar(string id)
        {
            var tasks = await _employeeService.GetEmployeeTasksAsync(id);
            var calendarEvents = tasks.Select(p => new CalendarEvent
            {
                Title = p.Title,
                EndDate = p.Deadline  // Direct assignment since both are DateOnly
            }).ToList();

            // Debug: Print events to console
           

            ViewBag.CalendarEvents = calendarEvents;
            return View();
        }
        public async Task<IActionResult> ProjectList()
        {
            var EmployeeJson = HttpContext.Session.GetString("User");
            var employeeId = JsonConvert.DeserializeObject<Employee>(EmployeeJson).Id;

            var projectCount = await _employeeService.GetProjectCountForEmployeeAsync(employeeId);
            var completedProjectsCount = await _employeeService.GetCompletedProjectsCountForEmployeeAsync(employeeId);
            var delayedProjectsCount = await _employeeService.GetDelayedProjectsCountForEmployeeAsync(employeeId);

            // Passer les statistiques dans ViewBag pour les afficher dans la vue
            ViewBag.ProjectCount = projectCount;
            ViewBag.CompletedProjectsCount = completedProjectsCount;
            ViewBag.DelayedProjectsCount = delayedProjectsCount;

            var projects = await _employeeService.GetEmployeeProjects(employeeId);
            return View(projects);
        }



    }

}
