using MGisbert.Appointments.Data.Enums;
using MGisbert.Appointments.Models;
using MGisbert.Appointments.Models.Request;
using MGisbert.Appointments.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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


        [Authorize]
        [HttpPost("user")]
        public async Task<IActionResult> AddUserAppointment([FromBody] AppointmentRequest appointmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserIdFromClaims();
            if (userId != appointmentRequest.UserId)
            {
                return BadRequest("User ID mismatch.");
            }
            appointmentRequest.UserId = userId;

            var appointment = await _appointmentService.AddAppointmentAsync(appointmentRequest);
            return Created("", appointment);
        }

        [Authorize]
        [HttpDelete("{id}/user")]
        public async Task<IActionResult> DeleteUserAppointment(int id)
        {
            var userIdFromClaims = GetUserIdFromClaims();

            await _appointmentService.DeleteAppointmentAsync(id, userIdFromClaims);
            return NoContent();
        }

        [Authorize]
        [HttpGet("{id}/user/")]
        public async Task<IActionResult> GetUserAppointment(int id)
        {
            var userIdFromClaims = GetUserIdFromClaims();

            var appointment = await _appointmentService.GetAppointmentAsync(id, userIdFromClaims);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserAppointments([FromQuery] string sortBy = "Date", [FromQuery] bool ascending = true)
        {
            var userIdFromClaims = GetUserIdFromClaims();

            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userIdFromClaims, sortBy, ascending);
            return Ok(appointments);
        }

        [Authorize]
        [HttpPut("{id}/user")]
        public async Task<IActionResult> UpdateUserAppointment(int id, [FromBody] Appointment appointment)
        {
            var userIdFromClaims = GetUserIdFromClaims();

            if (id != appointment.Id)
            {
                return BadRequest("Appointment ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userIdFromClaims != appointment.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            var updatedAppointment = await _appointmentService.UpdateAppointmentAsync(appointment, userIdFromClaims);
            return Ok(updatedAppointment);
        }

        #region Manager Endpoints
        [Authorize(Roles = "Manager")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var updatedAppointment = await _appointmentService.UpdateAppointmentStatusAsync(id, Status.Approved);
            return Ok(updatedAppointment);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var updatedAppointment = await _appointmentService.UpdateAppointmentStatusAsync(id, Status.Cancelled);
            return Ok(updatedAppointment);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments([FromQuery] string sortBy = "Date", [FromQuery] bool ascending = true)
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync(sortBy, ascending);
            return Ok(appointments);
        }
        #endregion

        #region private methods
        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim.Value);
        }
        #endregion
    }
}
