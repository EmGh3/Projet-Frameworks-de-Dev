// Licensed to the .NET Foundation under one or more agreements.
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
            ILogger<RegisterModel> logger
           , IDepartmentRepository depRepo,ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
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
            [Required(ErrorMessage = "First name is required")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Date of birth is required")]
            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            public DateOnly DateOfBirth { get; set; }

            [Required(ErrorMessage = "Gender is required")]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Phone number is required")]
            [Phone(ErrorMessage = "Invalid phone number")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Address is required")]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Department is required")]
            [Display(Name = "Department")]
            public int DepartmentId { get; set; }

            [Required(ErrorMessage = "Designation is required")]
            [Display(Name = "Designation")]
            public string Designation { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async System.Threading.Tasks.Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Departments = await _depRepo.GetAllAsync();
            ViewData["Departments"] = new SelectList(Departments, "Id", "Name");
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
                    _logger.LogError("ModelState error: " + error.ErrorMessage);
                }
                return Page(); 
            }

            _logger.LogInformation("ModelState is valid, continuing registration process.");

            var user = CreateUser() as Employee;

            if (user == null)
            {
                _logger.LogError("Unable to create employee."); 
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
                    _logger.LogError("Registration failed: " + error.Description); 
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page(); 
            }

            _logger.LogInformation("User created successfully.");

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

            await _userManager.AddToRoleAsync(user, "Employee");
            if (user != null)
            {
               
                if (user.Discriminator == "ProjectManager")
                {
                    var manager = await _context.ProjectManagers
                        .FirstOrDefaultAsync(pm => pm.Id == user.Id);

                    var managerJson = JsonConvert.SerializeObject(manager);
                    HttpContext.Session.SetString("isManager", "true");  
                    HttpContext.Session.SetString("User", managerJson);  
                }
                else if (user.Discriminator == "Employee")
                {
                    var employee = await _context.Employees
                        .FirstOrDefaultAsync(emp => emp.Id == user.Id);

                    var employeeJson = JsonConvert.SerializeObject(employee);
                    HttpContext.Session.SetString("isManager", "false");  
                    HttpContext.Session.SetString("User", employeeJson);  
                }
            }
            return LocalRedirect(returnUrl);





        }



        private User CreateUser()
        {
            try
            {
               
                    return Activator.CreateInstance<Employee>(); 
               
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance ");
            }
        }

      
    }
}
