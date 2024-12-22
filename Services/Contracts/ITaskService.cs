using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface ITaskService : IGenericService<ProjectTask>
    {
        public ProjectTask GetByIdWithIncludes(int id);
    }
}
