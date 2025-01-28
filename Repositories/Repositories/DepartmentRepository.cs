using ERP_Project.Data;
using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Repositories.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
           
        }

        public Department GetDepartmentByName(string name)
        {
            return _dbSet.FirstOrDefault(d => d.Name == name);

        }
    }

    
    
}
