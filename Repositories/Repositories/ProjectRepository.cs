using ERP_Project.Data;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Repositories.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext db) : base(db)
        {
        }
        public IEnumerable<ProjectTask> GetTasks(int projectId)
        {
            return _dbSet.Include(p => p.Tasks).SingleOrDefault(p => p.Id == projectId).Tasks;
        }
        public IEnumerable<Project> GetByProjectManagerId(string projectManagerId)
        {
            return _dbSet.Include(p => p.Tasks).Where(p => p.ProjectManagerId == projectManagerId).ToList();
        }



    }
}
