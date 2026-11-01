using HealthCareApp.Data;
using HealthCareApp.Dtos.AppointmentDtos;
using HealthCareApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAppointments")]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await _context.Appointments.ToListAsync();
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found.");
            }

            var response = appointments.Select(a => new GetAppointmentResponse
            {
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                AppointmentDate = a.AppointmentDate,
            });
            return Ok(response);
        }

        [HttpGet("GetAppointmentById/{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }
            var response = new GetAppointmentResponse
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
            };
            return Ok(response);
        }

        [HttpPost("CreateAppointment")]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentRequest request)
        {
            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
            };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            var response = new CreateAppointmentRequest
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
            };
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, response);
        }
             
        [HttpPut("UpdateAppointment/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, UpdateAppointmentRequest request)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }

            appointment.PatientId = request.PatientId;
            appointment.DoctorId = request.DoctorId;
            appointment.AppointmentDate = request.AppointmentDate;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            var response = new UpdateAppointmentRequest
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
            };
            return Ok(response);
        }
        [HttpDelete("DeleteAppointment/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok($"Appointment with ID {id} has been deleted.");
        }
    }
}
