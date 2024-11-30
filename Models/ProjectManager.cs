namespace ERP_Project.Models
{
    public class ProjectManager:User
    {
        public ICollection<Project> Projects { get; set; }

    }
}
