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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
