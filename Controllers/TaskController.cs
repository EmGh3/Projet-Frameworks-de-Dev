using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ERP_Project.Models;
using ERP_Project.Services.Services;
using ERP_Project.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.CodeAnalysis;



namespace ERP_Project.Controllers
{
    [Authorize]
    public class TaskController: Controller
    {
        public readonly ITaskService _taskService;
        public readonly IProjectService _projectService;
        public readonly ICommentService _commentService;
        public readonly IEmployeeService _employeeService;
        public readonly UserManager<User> _userManager;
        public TaskController(ITaskService taskService,ICommentService commentService , IProjectService projectService, UserManager<User> userManager, IEmployeeService employeeService)
        {
            _taskService = taskService;
            _commentService = commentService;
            _projectService = projectService;
            _userManager = userManager;
            _employeeService = employeeService;
        }
        [Authorize(Roles = "ProjectManager")]
        [HttpGet]
        [Route("Task/Create")]
        //Allow the manager to choose the project when creating a task
        public IActionResult Create()
        {
            ViewBag.projectList = new SelectList(_projectService.GetByProjectManagerId(_userManager.GetUserId(User)), "Id", "Name");
            return View();
        }
        [Authorize(Roles = "ProjectManager")]
        [HttpGet]
        [Route("Task/Create/{id:int}")]
        //Create a task for a specific project
        public IActionResult Create(int id)
        {
            var project = _projectService.GetByIdAsync(id).Result;
            if (project == null)
            {
                return NotFound();
            }
            ViewBag.project = project;
            ViewBag.ProjectId = id; 
            return View();
        }
        [Authorize(Roles = "ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask projectTask)
        {
            projectTask.Id = 0;
            projectTask.Status = ProjectTaskStatus.NotStarted;
            projectTask.StartDate = DateOnly.FromDateTime(DateTime.Now);
            await _taskService.AddAsync(projectTask);
            _projectService.UpdateProgress(projectTask.ProjectId);
            _projectService.UpdateExpenses(projectTask.ProjectId);
            return RedirectToAction("Details", new { projectTask.Id }); 
        }
        public IActionResult Details(int id)
        { 
            ViewData["UserId"] = _userManager.GetUserId(User);
            ProjectTask projectTask = _taskService.GetByIdWithIncludes(id);
            IEnumerable<Comment> comments = _commentService.GetByTaskId(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            ViewData["Comments"] = comments;
            return View(projectTask);
        }
        [Authorize(Roles = "ProjectManager")]
        public IActionResult Delete(int id)
        {
            ProjectTask task = _taskService.GetByIdAsync(id).Result;
            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Delete(ProjectTask task)
        {
            await _taskService.DeleteAsync(task.Id);
            _projectService.UpdateProgress(task.ProjectId);
            _projectService.UpdateExpenses(task.ProjectId);
            return RedirectToAction(nameof(ListByManager));
        }
        public IActionResult ListByProject(int id)
        {
            ViewData["UserId"] = _userManager.GetUserId(User);
            ViewData["ProjectId"] = id;
            ViewData["ProjectName"] = _projectService.GetByIdAsync(id).Result.Name;
            IEnumerable<ProjectTask> projectTasks = _taskService.GetByProjectId(id);
            return View(projectTasks);
        }
        [Authorize(Roles = "ProjectManager")]
        public IActionResult ListByManager()
        {
            ViewData["UserId"] = _userManager.GetUserId(User);
            IEnumerable<ProjectTask> projectTasks = _taskService.GetByProjectManagerId(_userManager.GetUserId(User));
            return View(projectTasks);
        }
        [Authorize(Roles = "Employee")]
        public IActionResult FinishTask (int id) {
            var projectTask = _taskService.GetByIdAsync(id).Result;
            if (projectTask == null)
            {
                return NotFound(new { Message = "Task not found." });
            }

            if (projectTask.EmployeeId != _userManager.GetUserId(User))
            {
                return Unauthorized(new { Message = "You are not authorized to update this task." });
            }
            _taskService.ChangeStatus(projectTask, ProjectTaskStatus.Finished);
            _projectService.UpdateProgress(projectTask.ProjectId);
            return RedirectToAction("Details", new { id });
        }
        [Authorize(Roles = "ProjectManager")]
        public IActionResult ChooseEmployee(int id)
        {
            var projectTask = _taskService.GetByIdAsync(id).Result;
            if (projectTask == null)
            {
                return NotFound(new { Message = "Task not found." });
            }
            var employees = _employeeService.GetAllAsync().Result
             .Select(e => new SelectListItem
             {
                Value = e.Id,  
                Text = $"{e.FirstName} {e.LastName}"
             })
            .ToList();
            ViewBag.Employees = employees;
            return View(projectTask);
        }
        [Authorize(Roles = "ProjectManager")]
        public IActionResult AssignTask(string employeeId, int taskId)
        {
            var task = _taskService.GetByIdAsync(taskId).Result;
            var employee = _employeeService.GetById(employeeId).Result;
            if (task == null)
            {
                return NotFound(new { Message = "Task not found."});
            }
            if (employee == null)
            {
                return NotFound(new { Message = "Employee not found." });
            }
            _taskService.ChangeStatus(task, ProjectTaskStatus.InProgress).Wait();
            _taskService.AssignTask(task, employee).Wait();
            _projectService.AddEmployeeToProject(task.ProjectId, employee);
            return RedirectToAction("Details", new { id = taskId });
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult RefuseTask(int id)
        {
            var task = _taskService.GetByIdAsync(id).Result;
            if (task == null)
            {
                return NotFound(new { Message = "Task not found." });
            }
            if (task.EmployeeId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }
            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RefuseTask(ProjectTask task)
        {
            _taskService.RemoveEmployee(task).Wait();
            _taskService.ChangeStatus(task, ProjectTaskStatus.NotStarted);
            return RedirectToAction("Details", new { task.Id });
        }
        public IActionResult ListByEmployee()
        {
            IEnumerable<ProjectTask> projectTasks = _taskService.GetByEmployeeId(_userManager.GetUserId(User));
            return View(projectTasks);
        }
        public IActionResult Update(int id)
        {
            ProjectTask task = _taskService.GetByIdAsync(id).Result;
            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProjectTask task)
        {
            await _taskService.UpdateAsync(task);
            return RedirectToAction("Details", new { task.Id });
        }
    }
}
