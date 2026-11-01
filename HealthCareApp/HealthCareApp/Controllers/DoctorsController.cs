using HealthCareApp.Data;
using HealthCareApp.Dtos.DoctorDtos;
using HealthCareApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetDoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Doctors.ToListAsync();
            if (doctors == null || doctors.Count == 0)
            {
                return NotFound("No doctors found.");
            }

            var response = doctors.Select(d => new GetDoctorResponse
            {
                Id = d.Id,
                Name = d.Name,
                Specialty = d.Specialty,
            });
            return Ok(response);
        }

        [HttpGet("GetDoctorById/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }
            var response = new GetDoctorResponse
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialty = doctor.Specialty,
            };
            return Ok(response);
        }

        [HttpPost("CreateDoctor")]
        public async Task<IActionResult> CreateDoctor(CreateDoctorRequest request)
        {
            var doctor = new Doctor
            {
                Name = request.Name,
                Specialty = request.Specialty,
            };
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            var response = new CreateDoctorRequest
            {
                Name = doctor.Name,
                Specialty = doctor.Specialty,
            };
            return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, response);
        }
        [HttpPut("UpdateDoctor/{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, UpdateDoctorRequest request)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }

            doctor.Name = request.Name;
            doctor.Specialty = request.Specialty;

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            var response = new UpdateDoctorRequest
            { 
                Name = doctor.Name,
                Specialty = doctor.Specialty,
            };
            return Ok(response);
        }
        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }
            doctor.IsDeleted = true;
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            return Ok($"Doctor with ID {id} has been deleted.");
        }
    }

}
