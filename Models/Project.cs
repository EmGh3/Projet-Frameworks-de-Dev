namespace ERP_Project.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate  { get; set; }
        public DateOnly? ActualEndDate { get; set; }
        public string Status { get; set; } // En cours, Terminé, etc.
        public double Progress { get; set; } 

        public decimal Budget { get; set; }
        public decimal Expenses { get; set; }

        public string ProjectManagerId { get; set; }
        public ProjectManager ProjectManager { get; set; } 

        public ICollection<Employee> Employees { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
