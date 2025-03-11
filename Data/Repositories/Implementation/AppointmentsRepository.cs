using MGisbert.Appointments.Data.Entities;

namespace MGisbert.Appointments.Data.Repositories.Implementation
{
    public class AppointmentsRepository : BaseRepository<Appointment>, IAppointmentsRepository
    {
        public AppointmentsRepository(Context context) : base(context)
        {        
        }
    }
}
