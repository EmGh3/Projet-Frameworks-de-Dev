namespace ERP_Project.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public int TaskId { get; set; }
        public ProjectTask Task { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
