using System.ComponentModel.DataAnnotations;

namespace ERP_Project.Models.viewModels
{
    public class UpdateEmployeeViewModel
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public int DepartmentId { get; set; }

        public string Designation { get; set; }
    }
}
