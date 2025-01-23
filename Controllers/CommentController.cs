using ERP_Project.Models;
using ERP_Project.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace ERP_Project.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        public readonly ICommentService _commentService;
        public readonly ITaskService _taskService;
        public readonly UserManager<User> _userManager;
        public CommentController(ICommentService commentService, ITaskService taskService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _taskService = taskService;
            _userManager = userManager;
        }
        public IActionResult Create(int id)
        {
            ProjectTask task = _taskService.GetByIdAsync(id).Result;
            if (task == null)
            {
                // Handle the case when the project is not found
                return NotFound();
            }
            ViewBag.TaskId = id; // Pass the id to the view
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            comment.Id = 0;
            comment.UserId = _userManager.GetUserId(User);
            await _commentService.AddAsync(comment);
            // Redirect to details page after successful creation
            return RedirectToAction("Details", "Task" ,new { id = comment.TaskId });
        }
        public async Task<IActionResult> Delete(int id)
        {
            Comment comment = await _commentService.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            if (comment.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }
            await _commentService.DeleteAsync(id);
            return RedirectToAction("Details", "Task", new { id = comment.TaskId });
        }
    }
}
