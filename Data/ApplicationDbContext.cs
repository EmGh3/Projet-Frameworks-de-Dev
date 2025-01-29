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
       

        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProjectManager> ProjectManagers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Project>()
            .HasOne(p => p.ProjectManager)
            .WithMany()
            .HasForeignKey(p => p.ProjectManagerId)
            .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")  
                .HasValue<User>("User")
                .HasValue<Employee>("Employee")
                .HasValue<ProjectManager>("ProjectManager");
            modelBuilder.Entity<ProjectTask>()
                  .HasOne(task => task.Employee)
                  .WithMany()
                  .HasForeignKey(task => task.EmployeeId)
                  .OnDelete(DeleteBehavior.SetNull);  

            modelBuilder.Entity<Comment>()
                 .HasOne(c => c.User)
                 .WithMany() 
                 .HasForeignKey(c => c.UserId); 
        }

    }
    }
