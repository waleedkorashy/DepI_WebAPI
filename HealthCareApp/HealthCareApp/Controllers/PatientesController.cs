using HealthCareApp.Data;
using HealthCareApp.Dtos.PatientDtos;
using HealthCareApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetPatients")]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            if (patients == null || patients.Count == 0)
            {
                return NotFound("No patients found.");
            }

            var response = patients.Select(p => new GetPatientResponse
            {
                Id = p.Id,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth,
            });
            return Ok(response);
        }

        [HttpGet("GetPatientById/{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }
            var response = new GetPatientResponse
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = patient.DateOfBirth,
            };
            return Ok(response);
        }
        
        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient(CreatePatientRequest request)
        {
            var patient = new Patient
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            var response = new CreatePatientRequest
            {
                Name = patient.Name,
                DateOfBirth = patient.DateOfBirth,
            };
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, response);
        }
                
        [HttpPut("UpdatePatient/{id}")]
        public async Task<IActionResult> UpdatePatient(int id, UpdatePatientRequest request)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            patient.Name = request.Name;
            patient.DateOfBirth = request.DateOfBirth;

            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            var response = new UpdatePatientRequest
            {
                Name = patient.Name,
                DateOfBirth = patient.DateOfBirth,
            };
            return Ok(response);
        }
        [HttpDelete("DeletePatient/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }
            patient.IsDeleted = true;
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            return Ok($"Patient with ID {id} has been deleted.");
        }
    }
}
