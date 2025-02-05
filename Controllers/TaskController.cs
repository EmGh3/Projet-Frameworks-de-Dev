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
        public async Task<IActionResult> Create()
        {
            ViewBag.projectList = new SelectList(await _projectService.GetByProjectManagerId(_userManager.GetUserId(User)), "Id", "Name");
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
            await _projectService.UpdateProgress(projectTask.ProjectId);
            await _projectService.UpdateExpenses(projectTask.ProjectId);
            return RedirectToAction("Details", new { projectTask.Id }); 
        }
        public async Task<IActionResult> Details(int id)
        { 
            ViewData["UserId"] = _userManager.GetUserId(User);
            ProjectTask projectTask = await _taskService.GetByIdWithIncludes(id);
            IEnumerable<Comment> comments =await _commentService.GetByTaskId(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            ViewData["Comments"] = comments;
            return View(projectTask);
        }
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Delete(int id)
        {
            ProjectTask task = await _taskService.GetByIdAsync(id);
            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProjectTask task)
        {
            var projectId = task.ProjectId;
            await _taskService.DeleteAsync(task.Id);
            await _projectService.UpdateExpenses(projectId);
            await _projectService.UpdateProgress(projectId);
            return RedirectToAction(nameof(ListByManager));
        }
        public async Task<IActionResult> ListByProject(int id)
        {
            ViewData["UserId"] = _userManager.GetUserId(User);
            ViewData["ProjectId"] = id;
            ViewData["ProjectName"] = _projectService.GetByIdAsync(id).Result.Name;
            IEnumerable<ProjectTask> projectTasks = await _taskService.GetByProjectId(id);
            return View(projectTasks);
        }
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> ListByManager()
        {
            ViewData["UserId"] = _userManager.GetUserId(User);
            IEnumerable<ProjectTask> projectTasks = await _taskService.GetByProjectManagerId(_userManager.GetUserId(User));
            return View(projectTasks);
        }
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> FinishTask (int id) {
            var projectTask = _taskService.GetByIdAsync(id).Result;
            if (projectTask == null)
            {
                return NotFound(new { Message = "Task not found." });
            }

            if (projectTask.EmployeeId != _userManager.GetUserId(User))
            {
                return Unauthorized(new { Message = "You are not authorized to update this task." });
            }
            await _taskService.ChangeStatus(projectTask, ProjectTaskStatus.Finished);
            await _projectService.UpdateProgress(projectTask.ProjectId);
            return RedirectToAction("Details", new { id });
        }
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> ChooseEmployee(int id)
        {
            var projectTask = await _taskService.GetByIdAsync(id);
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
        public async Task<IActionResult> AssignTask(string employeeId, int taskId)
        {
            var task = await _taskService.GetByIdAsync(taskId);
            var employee = await _employeeService.GetById(employeeId);
            if (task == null)
            {
                return NotFound(new { Message = "Task not found."});
            }
            if (employee == null)
            {
                return NotFound(new { Message = "Employee not found." });
            }
            await _taskService.ChangeStatus(task, ProjectTaskStatus.InProgress);
            await _taskService.AssignTask(task, employee);
            await _projectService.AddEmployeeToProject(task.ProjectId, employee);
            return RedirectToAction("Details", new { id = taskId });
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> RefuseTask(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
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
        public async Task<IActionResult> RefuseTask(ProjectTask task)
        {
            await _taskService.RemoveEmployee(task);
            await _taskService.ChangeStatus(task, ProjectTaskStatus.NotStarted);
            return RedirectToAction("Details", new { task.Id });
        }
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ListByEmployee()
        {
            IEnumerable<ProjectTask> projectTasks = await _taskService.GetByEmployeeId(_userManager.GetUserId(User));
            return View(projectTasks);
        }
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Update(int id)
        {
            ProjectTask task = await _taskService.GetByIdAsync(id);
            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProjectTask task)
        {
            await _taskService.UpdateAsync(task);
            await _projectService.UpdateExpenses(task.ProjectId);
            return RedirectToAction("Details", new { task.Id });
        }
    }
}
