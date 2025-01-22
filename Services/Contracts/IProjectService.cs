using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface IProjectService : IGenericService<Project>
    {
        public IEnumerable<ProjectTask> GetTasks(int projectId);
        public void UpdateProgress(int projectId);
    }
}
