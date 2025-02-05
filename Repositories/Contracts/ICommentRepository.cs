using ERP_Project.Models;

namespace ERP_Project.Repositories.Contracts
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public Task<IEnumerable<Comment>> GetByTaskId(int taskId);
    }
}
