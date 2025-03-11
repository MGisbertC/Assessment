using MGisbert.Appointments.Data.Enums;

namespace MGisbert.Appointments.Data.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Status status { get; set; } = Status.Pending;
        public DateTime Date { get; set; }
        public int UserId { get; set; }


        //Navigation properties
        public User User { get; set; }
    }
}
