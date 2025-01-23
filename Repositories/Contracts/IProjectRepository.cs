using ERP_Project.Models;
using System.Threading.Tasks;

namespace ERP_Project.Repositories.Contracts
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        public IEnumerable<ProjectTask> GetTasks(int projectId);
        public IEnumerable<Project> GetByProjectManagerId(string projectManagerId);


    }
}
