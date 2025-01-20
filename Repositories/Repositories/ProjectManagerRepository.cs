using ERP_Project.Data;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Repositories.Repositories
{
    public class ProjectManagerRepository: GenericRepository<ProjectManager>, IProjectManagerRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectManagerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;


        }

       

      

        public async Task<ProjectManager> GetByIdAsync(string id)
        {
            return await _context.ProjectManagers
                .FirstOrDefaultAsync(p => p.Id == id);
        }


    }
}
