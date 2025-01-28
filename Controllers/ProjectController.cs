using ERP_Project.Models;
using ERP_Project.Models.ViewModels;
using ERP_Project.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ERP_Project.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IProjectManagerService _projectManagerService;

        public ProjectController(IProjectService projectService, IProjectManagerService projectManagerService)
        {
            _projectService = projectService;
            _projectManagerService = projectManagerService;
        }

        // GET: Project/Index
        public async Task<IActionResult> Index()
        {
            
            var managerJson = HttpContext.Session.GetString("User");
            var managerId = JsonConvert.DeserializeObject<ProjectManager>(managerJson).Id;

            var projectCount = _projectManagerService.GetProjectCountForUser(managerId);
            ViewBag.ProjectCount = projectCount;

            var completedProjectsCount = await _projectManagerService.GetCompletedProjectsCountForUserAsync(managerId);
            ViewBag.CompletedProjectsCount = completedProjectsCount;

            var delayedProjectsCount = await _projectService.GetDelayedProjectsCountForUserAsync(managerId);
            ViewBag.DelayedProjectsCount = delayedProjectsCount;

            var projects = await _projectService.GetProjectsByManagerAsync(managerId);
            return View(projects);
        }
        // GET: Project/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetProjectWithTasksAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);  // Passing the project with tasks to the view
        }
        
        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await _projectService.GetProjectWithTasksAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateProjectViewModel();

            // Fetch the logged-in Project Manager
            var managerJson = HttpContext.Session.GetString("User");
            var managerId = JsonConvert.DeserializeObject<ProjectManager>(managerJson).Id;
            var projectManager = await _projectManagerService.GetByIdAsync(managerId);

            // Set the Project Manager as a readonly input (not editable)
            viewModel.ProjectManagerUsername = projectManager.UserName;

            viewModel.StartDate = new DateOnly(2025, 1, 1);
            viewModel.EndDate = new DateOnly(2025, 1, 1);

            return View(viewModel);
        }





        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectViewModel viewModel)
        {

            var managerJson = HttpContext.Session.GetString("User");
            var managerId = JsonConvert.DeserializeObject<ProjectManager>(managerJson).Id;
            var projectManager = await _projectManagerService.GetByIdAsync(managerId);

            var project = new Project
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    Budget = viewModel.Budget,
                    Status = "In Progress",
                    Expenses = 0,
                    Progress = 0,
                    ActualEndDate = null,
                    ProjectManagerId = managerId,
                    ProjectManager = projectManager// Here, you're assigning it correctly
                };

                await _projectService.AddAsync(project);

                return RedirectToAction(nameof(Index));
            
           
            
            
            
        }

        // GET: Project/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var viewModel = new CreateProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                ProjectManagerId = project.ProjectManagerId // Keep the current Project Manager ID
            };

            // Fetch the logged-in Project Manager
            var managerJson = HttpContext.Session.GetString("User");
            var managerId = JsonConvert.DeserializeObject<ProjectManager>(managerJson).Id;
            var projectManager = await _projectManagerService.GetByIdAsync(managerId);

            // Set the Project Manager as a readonly input (not editable)
            viewModel.ProjectManagerUsername = projectManager.UserName;

            return View(viewModel);
        }




        // ... other code ...

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateProjectViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Update the project with the data from the ViewModel
            project.Name = viewModel.Name;
            project.Description = viewModel.Description;
            project.StartDate = viewModel.StartDate;
            project.EndDate = viewModel.EndDate;
            project.Budget = viewModel.Budget;

            await _projectService.UpdateAsync(project);

            var lastUrl = HttpContext.Session.GetString("LastUrl");
            if (lastUrl != null)
            {
                return Redirect(lastUrl);
            }

            // If no referrer is available, fallback to Index
            return RedirectToAction(nameof(Index));
        }
        // GET: Project/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Project/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

       
    }
}
