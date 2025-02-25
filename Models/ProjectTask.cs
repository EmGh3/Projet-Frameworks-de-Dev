﻿namespace ERP_Project.Models
{
    public enum ProjectTaskStatus
    {
        NotStarted,
        InProgress,
        Finished
    }
    public class ProjectTask
    {
       
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly Deadline { get; set; }
        public ProjectTaskStatus Status { get; set; } // En cours, Terminé, etc.


        public double Cost { get; set; }
        public int ProjectId { get; set; }

        public Project Project { get; set; }
        public string? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
