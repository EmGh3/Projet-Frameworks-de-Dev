namespace ERP_Project.Models
{
    public class Employee:User
    {
        public string Designation { get; set; }
  
        public int DepartmentId { get; set; }

        
        public Department Department { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; }


    }
}
