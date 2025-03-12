using MGisbert.Appointments.Data.Enums;
using MGisbert.Appointments.Models;
//using Microsoft.EntityFrameworkCore;

namespace MGisbert.Appointments.Services.Implementation
{
    public partial class AppointmentService : IAppointmentService
    {
        public async Task<Appointment> UpdateAppointmentStatusAsync(int id, Status status)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAsync(id);
                if (appointment == null)
                {
                    throw new KeyNotFoundException("Appointment not found.");
                }

                if (status == Status.Approved && appointment.Status != Status.Pending)
                {
                    throw new InvalidOperationException("Only pending appointments can be approved.");
                }

                if (status == Status.Cancelled && appointment.Status == Status.Cancelled)
                {
                    throw new InvalidOperationException("Appointment already cancelled.");
                }

                appointment.Status = status;
                await _appointmentRepository.UpdateAsync(appointment);

                return _mapper.Map<Appointment>(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
            }
        }
        public async Task DeleteAppointmentAsync(int id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAsync(id);
                if (appointment == null)
                {
                    throw new KeyNotFoundException("Appointment not found.");
                }

                if (appointment.Status != Status.Cancelled)
                {
                    throw new InvalidOperationException("Only cancelled appointments can be deleted.");
                }

                await _appointmentRepository.DeleteAsync(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
            }
        }    
        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(string sortBy, bool ascending)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAsync();

                switch (sortBy.ToLower())
                {
                    case "date":
                        appointments = ascending ? appointments.OrderBy(a => a.Date) : appointments.OrderByDescending(a => a.Date);
                        break;
                    case "status":
                        appointments = ascending ? appointments.OrderBy(a => a.Status) : appointments.OrderByDescending(a => a.Status);
                        break;
                    default:
                        throw new ArgumentException("Invalid sort parameter.");
                }

                //if (!string.IsNullOrEmpty(sortBy))
                //{
                //    appointments = ascending
                //        ? appointments.OrderBy(a => EF.Property<object>(a, sortBy))
                //        : appointments.OrderByDescending(a => EF.Property<object>(a, sortBy));
                //    return _mapper.Map<IEnumerable<Appointment>>(appointments);
                //}

                return _mapper.Map<IEnumerable<Appointment>>(appointments);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
            }
        }       
    }
}
