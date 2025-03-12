using MGisbert.Appointments.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MGisbert.Appointments.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; } = Status.Pending;
        [Required]
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
