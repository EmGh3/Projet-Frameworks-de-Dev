﻿using ERP_Project.Data;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Repositories.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Example of a specific method for the Project entity
        public async Task<IEnumerable<Project>> GetProjectsWithDetailsAsync()
        {
            return await _dbSet
                .Include(p => p.ProjectManager)
                .Include(p => p.Employees)
                .Include(p => p.Tasks)
                .ToListAsync();
        }
        public async Task<IEnumerable<Project>> GetAllProjectsByManagerAsync(string projectManagerId)
        {
            return await _context.Projects
                .Where(p => p.ProjectManagerId == projectManagerId)
                .ToListAsync();
        }
        public async Task<int> GetDelayedProjectsCountAsync(string userId)
        {
            var count = await _context.Projects
                .Where(p => p.ProjectManagerId == userId && p.EndDate < DateOnly.FromDateTime(DateTime.Now)
                            && ( p.ActualEndDate > p.EndDate)
                            && p.Status != "Completed")  // Assuming "Terminé" is the status for completed projects
                .CountAsync();

            return count;
        }
        public async Task<Project> GetProjectWithTasksAsync(int projectId)
        {
            // Fetch the project along with its tasks using Include
            return await _context.Projects
                .Include(p => p.Tasks)  // Include tasks related to the project
                .FirstOrDefaultAsync(p => p.Id == projectId);  // Filter by the project ID
        }
        public IEnumerable<ProjectTask> GetTasks(int projectId)
        {
            return _dbSet.Include(p => p.Tasks).SingleOrDefault(p => p.Id == projectId).Tasks;
        }
        public IEnumerable<Project> GetByProjectManagerId(string projectManagerId)
        {
            return _dbSet.Include(p => p.Tasks).Where(p => p.ProjectManagerId == projectManagerId).ToList();
        }
        public void AddEmployeeToProject(int projectId, Employee employee)
        {
            var project = _dbSet.Include(p => p.Employees).FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                project.Employees.Add(employee);
                _context.SaveChanges(); // Save changes to persist the addition
            }
        }

    }
}
