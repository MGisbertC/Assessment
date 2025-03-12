using MGisbert.Appointments.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MGisbert.Appointments.Models.Request
{
    public class AppointmentRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150)]
        public required string Title { get; set; }
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
