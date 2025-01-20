namespace ERP_Project.Models.viewModels
{
    public class ProjectManagerDetailsViewModel
    {
        public ProjectManager Manager { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public List<CalendarEvent> CalendarEvents { get; set; }
    }

    public class CalendarEvent
    {
        public string Title { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }

}
