using System;
using System.ComponentModel.DataAnnotations;

namespace IT15_Project.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string? AssignedDoctor { get; set; }

        public string? AssignedDept { get; set; }

        public string Status { get; set; } = "Paid";
        public string? Validity { get; set; } = "Active";

    }
}
