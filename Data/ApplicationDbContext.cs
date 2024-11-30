using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERP_Project.Models;

namespace ERP_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       

        DbSet<Department> Departments { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<ProjectManager> ProjectManagers { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectTask> Tasks { get; set; }
        DbSet<Employee> Employees { get; set; }

    }
    }
