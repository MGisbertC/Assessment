using MGisbert.Appointments.Data.Enums;
using MGisbert.Appointments.Models;
using MGisbert.Appointments.Models.Request;
using MGisbert.Appointments.Services;
using Microsoft.AspNetCore.Mvc;

namespace MGisbert.Appointments.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("user")]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentRequest appointmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var appointment = await _appointmentService.AddAppointmentAsync(appointmentRequest);
                return Created("",appointment);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}/user/{userId}")]
        public async Task<IActionResult> DeleteAppointment(int id, int userId)
        {
            try
            {
                await _appointmentService.DeleteAppointmentAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}/user/{userId}")]
        public async Task<IActionResult> GetAppointment(int id, int userId)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentAsync(id, userId);
                if (appointment == null)
                {
                    return NotFound();
                }
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAppointments(int userId, [FromQuery]string sortBy ="Date", [FromQuery]bool ascending = true)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId, sortBy, ascending);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/user/{userId}")]
        public async Task<IActionResult> UpdateAppointment(int id, int userId, [FromBody] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest("Appointment ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedAppointment = await _appointmentService.UpdateAppointmentAsync(appointment, userId);
                return Ok(updatedAppointment);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        #region Manager Endpoints
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            try
            {
                var updatedAppointment = await _appointmentService.UpdateAppointmentStatusAsync(id, Status.Approved);
                return Ok(updatedAppointment);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                var updatedAppointment = await _appointmentService.UpdateAppointmentStatusAsync(id, Status.Cancelled);
                return Ok(updatedAppointment);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                await _appointmentService.DeleteAppointmentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments([FromQuery] string sortBy = "Date", [FromQuery] bool ascending = true)
        {
            try
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync(sortBy, ascending);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        #endregion

        #region Private methods
        private IActionResult HandleException(Exception ex)
        {
            return ex switch
            {
                KeyNotFoundException => NotFound(),
                InvalidOperationException => BadRequest(ex.Message),
                ApplicationException => StatusCode(StatusCodes.Status500InternalServerError, ex.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };
        }
        #endregion
    }
}
