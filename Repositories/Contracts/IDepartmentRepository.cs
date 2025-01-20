using ERP_Project.Models;
namespace ERP_Project.Repositories.Contracts
{
    public interface  IDepartmentRepository : IGenericRepository<Department>
    {
        Department GetDepartmentByName(string name);
    }
}
