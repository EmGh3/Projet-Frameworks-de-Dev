using ERP_Project.Models;

namespace ERP_Project.Services.Contracts
{
    public interface IDepartmentService:IGenericService<Department>
    {
        Department GetDepartmentByName(string name);
    }
}
