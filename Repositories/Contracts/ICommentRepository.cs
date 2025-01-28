using ERP_Project.Models;

namespace ERP_Project.Repositories.Contracts
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public IEnumerable<Comment> GetByTaskId(int taskId);
    }
}
