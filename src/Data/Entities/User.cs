namespace MGisbert.Appointments.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }

        //Navigation properties
        public Role Role { get; set; }
    }
}
