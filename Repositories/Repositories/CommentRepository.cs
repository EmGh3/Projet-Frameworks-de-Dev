using ERP_Project.Data;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Repositories.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext db) : base(db)
    {
    }
        public async Task<IEnumerable<Comment>> GetByTaskId(int taskId)
        {
            return _dbSet.Include(c => c.User).Where(c => c.TaskId == taskId).ToList();
        }
    }
}
