﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using ERP_Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ERP_Project.Models;
using ERP_Project.Repositories.Repositories;
using ERP_Project.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ERP_Project.Data;
namespace ERP_Project.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IDepartmentRepository _depRepo;
        private readonly ApplicationDbContext _context;
        public IEnumerable<Department> Departments { get; set; }
        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole>roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, IDepartmentRepository depRepo,ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _depRepo = depRepo;
            _context=dbContext;

        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // Add the additional fields from the User model
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Date of Birth")]
            public DateOnly DateOfBirth { get; set; }

            [Required]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            // Role Selection
           

            // Department and Designation (only shown when Role is "Employee")
            [Display(Name = "Department")]
            public int DepartmentId { get; set; }

            [Display(Name = "Designation")]
            public string Designation { get; set; }
        }

        public async System.Threading.Tasks.Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Departments = await _depRepo.GetAllAsync();
            ViewData["Departments"] = new SelectList(Departments, "Id", "Id");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError("ModelState error: " + error.ErrorMessage); // Log the ModelState errors
                }
                return Page(); // Ensure this is reached if ModelState is invalid
            }

            _logger.LogInformation("ModelState is valid, continuing registration process.");

            var user = CreateUser() as Employee;

            if (user == null)
            {
                _logger.LogError("Unable to create employee.");  // Log error if user creation fails
                ModelState.AddModelError(string.Empty, "Unable to create employee.");
                return Page();
            }

            _logger.LogInformation("Employee user created successfully.");

            user.UserName = Input.Email;
            user.Email = Input.Email;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.DateOfBirth = Input.DateOfBirth;
            user.Gender = Input.Gender;
            user.Address = Input.Address;
            user.PhoneNumber = Input.PhoneNumber;
            user.Designation = Input.Designation;
            user.DepartmentId = Input.DepartmentId;

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Registration failed: " + error.Description); // Log registration errors
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page(); // Ensure this returns errors if registration fails
            }

            _logger.LogInformation("User created successfully.");

            // Ensure the "Employee" role exists
            var roleExist = await _roleManager.RoleExistsAsync("Employee");
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Employee"));
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Failed to create 'Employee' role.");
                    ModelState.AddModelError(string.Empty, "Failed to create the role.");
                    return Page();
                }
            }

            // Add the selected role to the user
            await _userManager.AddToRoleAsync(user, "Employee");
            if (user != null)
            {
                // Check if the user is a ProjectManager or Employee
                if (user.Discriminator == "ProjectManager")
                {
                    // Retrieve the ProjectManager object
                    var manager = await _context.ProjectManagers
                        .FirstOrDefaultAsync(pm => pm.Id == user.Id);

                    // Serialize the manager object and store it in the session
                    var managerJson = JsonConvert.SerializeObject(manager);
                    HttpContext.Session.SetString("isManager", "true");  // Set the 'isManager' flag to true
                    HttpContext.Session.SetString("User", managerJson);  // Store the manager object in session
                }
                else if (user.Discriminator == "Employee")
                {
                    // Retrieve the Employee object
                    var employee = await _context.Employees
                        .FirstOrDefaultAsync(emp => emp.Id == user.Id);

                    // Serialize the employee object and store it in the session
                    var employeeJson = JsonConvert.SerializeObject(employee);
                    HttpContext.Session.SetString("isManager", "false");  // Set the 'isManager' flag to false
                    HttpContext.Session.SetString("User", employeeJson);  // Store the employee object in session
                }
            }




            // Email confirmation and sign-in process
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }



        private User CreateUser()
        {
            try
            {
                // Check the role and return the corresponding user type
               
                    return Activator.CreateInstance<Employee>(); // Create an Employee instance
               
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance ");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
