using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface ICommentService : IGenericService<Comment>
    {
        public Task<IEnumerable<Comment>> GetByTaskId(int taskId);
    }
}
