
    namespace ERP_Project.Models
    {
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; } // Add FromEmail property
        public string FromName { get; set; }  // Add FromName property
    }
}


