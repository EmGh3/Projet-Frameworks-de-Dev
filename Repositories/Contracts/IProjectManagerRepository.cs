using ERP_Project.Models;

namespace ERP_Project.Repositories.Contracts
{
    public interface IProjectManagerRepository: IGenericRepository<ProjectManager>
    {


        Task<ProjectManager> GetByIdAsync(string id);


    }
}
