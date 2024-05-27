using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace IT15_Project.Models
{
    public class DoctorModel
    {
        [Key]
        public int DoctorId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Department { get; set; }
        public required string BirthDate { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Gender { get; set; }
        public required string ImageFileName { get; set; }


    }
}
