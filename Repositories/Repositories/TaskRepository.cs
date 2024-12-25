using ERP_Project.Data;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Repositories.Repositories
{
    public class TaskRepository: GenericRepository<ProjectTask>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext db) : base(db)
        {
        }
        public ProjectTask GetByIdWithIncludes(int id)
        {
            return _dbSet.Include(t => t.Employee).Include(t => t.Project)
                .SingleOrDefault(t => t.Id == id);
        }
        public IEnumerable<ProjectTask> GetByProjectId(int projectId)
        {
            return _dbSet.Include(t => t.Project).Include(t=>t.Employee)
                .Where(t => t.ProjectId == projectId).ToList();
        }
    }
}
