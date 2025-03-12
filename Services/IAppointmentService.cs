using MGisbert.Appointments.Data.Enums;
using MGisbert.Appointments.Models;
using MGisbert.Appointments.Models.Request;
using System.Globalization;

namespace MGisbert.Appointments.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(int userId, string sortBy, bool ascending);
        Task<Appointment> GetAppointmentAsync(int id, int userId);
        Task<Appointment> AddAppointmentAsync(AppointmentRequest appointment);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment, int userId);
        Task DeleteAppointmentAsync(int id, int userId);
        Task<Appointment> UpdateAppointmentStatusAsync(int id, Status status);
        Task DeleteAppointmentAsync(int id);
    }
}
