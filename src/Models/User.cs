using MGisbert.Appointments.Data.Entities;

namespace MGisbert.Appointments.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
