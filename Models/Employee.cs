namespace ERP_Project.Models
{
    public class Employee:User
    {
        public string Designation { get; set; }
        // Foreign key
        public int DepartmentId { get; set; }

        // Navigation property
        public Department Department { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; }


    }
}
