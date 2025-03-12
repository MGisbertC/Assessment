using AutoMapper;
using MGisbert.Appointments.Data.Enums;
using MGisbert.Appointments.Data.Repositories;
using MGisbert.Appointments.Models;
using MGisbert.Appointments.Models.Request;
using System.Globalization;

namespace MGisbert.Appointments.Services.Implementation
{
    public partial class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IAppointmentRepository appointmentsRepository, IUserRepository userRepository, IMapper mapper, ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentsRepository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<Appointment> AddAppointmentAsync(AppointmentRequest appointment)
        {
            try
            {
                bool userExists = await _userRepository.GetAny(u => u.Id == appointment.UserId);
                if (!userExists)
                {
                    throw new ArgumentException("The specified user not exists.");
                }

                var requestMapped = _mapper.Map<Data.Entities.Appointment>(appointment);
                await _appointmentRepository.AddAsync(requestMapped);
                return _mapper.Map<Appointment>(requestMapped);
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

        public async Task DeleteAppointmentAsync(int id, int userId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetSingleASync(p => p.Id == id && p.UserId == userId);
                if (appointment == null)
                {
                    throw new KeyNotFoundException("Appointment not found.");
                }

                if (appointment.Status != Status.Pending)
                {
                    throw new InvalidOperationException("Only appointments with status 'Pending' can be deleted.");
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

        public async Task<Appointment> GetAppointmentAsync(int id, int userId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetSingleASync(p=>p.Id == id && p.UserId == userId);
                if (appointment == null)
                {
                    throw new KeyNotFoundException("Appointment not found.");
                }

                return _mapper.Map<Appointment>(appointment);
            }
            catch (KeyNotFoundException ex)
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

        public async Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(int userId, string sortBy, bool ascending)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAsync(u => u.UserId == userId);

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
                var asd = _mapper.Map<IEnumerable<Appointment>>(appointments);
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

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment, int userId)
        {
            try
            {
                var existingAppointment = await _appointmentRepository.GetSingleASync(p=>p.Id == appointment.Id && p.UserId == userId);
                if (existingAppointment == null)
                {
                    throw new KeyNotFoundException("Appointment not found.");
                }

                if (existingAppointment.Status != Status.Pending)
                {
                    throw new InvalidOperationException("Only appointments with status 'Pending' can be updated.");
                }

                _mapper.Map(appointment, existingAppointment);
                await _appointmentRepository.UpdateAsync(existingAppointment);
                return _mapper.Map<Appointment>(existingAppointment);
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
    }
}
