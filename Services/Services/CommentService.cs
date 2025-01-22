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
        // Get all the comments of a task by its ID
        public IEnumerable<Comment> GetByTaskId(int taskId)
        {
            return ((ICommentRepository)_repository).GetByTaskId(taskId);
        }
    }
}
