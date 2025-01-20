using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface IProjectManagerService
    {

      
            Task<ProjectManager> GetByIdAsync(string id);
        
    }
}
