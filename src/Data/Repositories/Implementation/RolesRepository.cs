using MGisbert.Appointments.Data.Entities;

namespace MGisbert.Appointments.Data.Repositories.Implementation
{
    public class RolesRepository : BaseRepository<Role>, IRoleRepository
    {
        public RolesRepository(Context context) : base(context)
        {        
        }
    }
}
