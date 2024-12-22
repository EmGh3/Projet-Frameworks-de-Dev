using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ERP_Project.Models;
using ERP_Project.Services.Services;
using ERP_Project.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;



namespace ERP_Project.Controllers
{
    public class TaskController: Controller
    {
        public readonly ITaskService _taskService;
        public readonly IGenericService<Project> _genericService;
        public TaskController(ITaskService taskService, IGenericService<Project> genericService)
        {
            _taskService = taskService;
            _genericService = genericService;
        }
        [Authorize(Roles = "ProjectManager")]
        public IActionResult Create(int id)
        {
            Project project = _genericService.GetByIdAsync(id).Result;
            if (project == null)
            {
                // Handle the case when the project is not found
                return NotFound();
            }
            ViewBag.project = project;
            ViewBag.ProjectId = id; // Pass the id to the view
            return View();
        }
        [Authorize(Roles = "ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask projectTask)
        {
            projectTask.Id = 0;
            projectTask.Status = ProjectTaskStatus.NotStarted;
            await _taskService.AddAsync(projectTask);
            // Redirect to details page after successful creation
            return RedirectToAction("Details", new { projectTask.Id }); 
        }
        public IActionResult Details(int id)
        {
            ProjectTask projectTask = _taskService.GetByIdWithIncludes(id);
            if (projectTask == null)
            {
                // Handle the case when the task is not found
                return NotFound();
            }
            return View(projectTask);
        }
        public IActionResult List(int id)
        {
            IEnumerable<ProjectTask> projectTasks = _taskService.GetByProjectId(id);
            return View(projectTasks);
        }
    }
}
