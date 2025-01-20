using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace ERP_Project.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }

        [Phone] public string PhoneNumber { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string Discriminator { get; set; }




    }
}
