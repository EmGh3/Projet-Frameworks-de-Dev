using ERP_Project.Models;
using ERP_Project.Repositories.Contracts;
using ERP_Project.Repositories.Repositories;
using ERP_Project.Services.Contracts;

namespace ERP_Project.Services.Services
{
    public class DepartmentService: GenericService<Department> ,IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository) : base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public Department GetDepartmentByName(string name)
        {
            return _departmentRepository.GetDepartmentByName(name);
        }
    }
}
