﻿using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ERP_Project.Models.viewModels;
using ERP_Project.Services.Contracts;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
namespace ERP_Project.Controllers
{
    [Authorize(Roles = "ProjectManager")]

    public class ProjectManagerController : Controller
    {
        public readonly IProjectManagerService _projectManagerService;
        private readonly UserManager<User> _userManager;

        public ProjectManagerController(IProjectManagerService projectManagerService, UserManager<User> userManager)
        {
            _projectManagerService = projectManagerService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var managers = await _projectManagerService.GetAllAsync();
            return View(managers); 
        }

        public async Task<IActionResult> ProjectManagerDetails()
        {
            var managerJson = HttpContext.Session.GetString("User");
            var managerId = JsonConvert.DeserializeObject<ProjectManager>(managerJson).Id;
            var manager = await _projectManagerService.GetByIdAsync(managerId);
            var projects = await _projectManagerService.GetProjectsByManager(manager.Id);

            var calendarEvents = projects.Select(p => new CalendarEvent
            {
                Title = p.Name, 
                StartDate = p.StartDate,
                EndDate = p.EndDate 
            }).ToList();

            var viewModel = new ProjectManagerDetailsViewModel
            {
                Manager = manager,
                Projects = projects,
                CalendarEvents = calendarEvents
            };

            return View(viewModel);
        }



        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            var viewModel = new UpdateProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Update user data
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("UpdateProfile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }

}

