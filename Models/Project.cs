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
        public double Progress { get; set; } // En pourcentage

        public decimal Budget { get; set; }
        public decimal Expenses { get; set; }

        public int ProjectManagerId { get; set; } // Foreign Key to Project Manager
        public ProjectManager ProjectManager { get; set; } // Navigation Property

        // Many-to-Many relationship with Employees
        public ICollection<Employee> Employees { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
