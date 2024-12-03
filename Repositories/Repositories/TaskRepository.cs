using ERP_Project.Data;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;

namespace ERP_Project.Repositories.Repositories
{
    public class TaskRepository: GenericRepository<ProjectTask>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
