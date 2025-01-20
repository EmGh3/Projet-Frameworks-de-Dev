namespace ERP_Project.Models.viewModels
{
    public class EmployeeProfileViewModel
    {
        public Employee Employee { get; set; } // Employee details
        public List<Project> Projects { get; set; } // List of associated projects
        public List<ProjectTask> Tasks { get; set; } // List of associated tasks
    }

}
