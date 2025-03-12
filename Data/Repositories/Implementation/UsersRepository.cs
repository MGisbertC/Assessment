using MGisbert.Appointments.Data.Entities;

namespace MGisbert.Appointments.Data.Repositories.Implementation
{
    public class UsersRepository : BaseRepository<User>, IUserRepository
    {
        public UsersRepository(Context context) : base(context)
        {        
        }
    }
}
