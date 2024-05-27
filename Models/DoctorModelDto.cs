using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace IT15_Project.Models
{
    public class DoctorModelDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = "";

        [Required, MaxLength(50)]
        public string LastName { get; set; } = "";

        [Required]
        public string Department { get; set; } = "";

        [Required, MaxLength(50)]
        public string BirthDate { get; set; } = "";

        [Required, MaxLength(11)]
        public string PhoneNumber { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Gender { get; set; } = "";

        public IFormFile? ImageFile { get; set; }
    }
}