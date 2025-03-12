using MGisbert.Appointments.Models;
using MGisbert.Appointments.Models.Request;

namespace MGisbert.Appointments.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginRequest loginRequest);
    }
}
