using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Services.Services
{
    public class CommentService : GenericService<Comment>,ICommentService
    {
        public CommentService(ICommentRepository repository) : base(repository)
        {

        }
        public IEnumerable<Comment> GetByTaskId(int taskId)
        {
            return ((ICommentRepository)_repository).GetByTaskId(taskId);
        }
    }
}
