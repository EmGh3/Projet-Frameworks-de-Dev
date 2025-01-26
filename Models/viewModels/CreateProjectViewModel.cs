using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_Project.Models.ViewModels
{
    public class CreateProjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; } = new DateOnly(2025, 1, 1); // Default value

        [Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; } = new DateOnly(2025, 1, 1); // Default value


        [Required(ErrorMessage = "Budget is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value.")]
        public decimal Budget { get; set; }

        
        public string ProjectManagerId { get; set; }

        // Add this property to store the Project Manager's username
        public string ProjectManagerUsername { get; set; }
    }
}
