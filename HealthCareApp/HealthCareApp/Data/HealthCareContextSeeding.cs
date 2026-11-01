using HealthCareApp.Data;
using HealthCareApp.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp
{
    internal class HealthCareContextSeeding
    {
        public async static Task SeedAsync(ApplicationDbContext dbContext)
        {
            var seedingDirectory = Path.Combine(AppContext.BaseDirectory, "Data", "Seeding");
            var serializerOptions = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            if (!await dbContext.Doctors.AnyAsync())
            {
                var doctorsData = File.ReadAllText(Path.Combine(seedingDirectory, "Doctors.json"));
                var doctors = JsonSerializer.Deserialize<List<Doctor>>(doctorsData, serializerOptions);

                if (doctors == null)
                    return;

                await dbContext.Doctors.AddRangeAsync(doctors);
                await dbContext.SaveChangesAsync();

            }
            if (!await dbContext.Patients.AnyAsync())
            {
                var patientsData = File.ReadAllText(Path.Combine(seedingDirectory, "Patients.json"));
                var patients = JsonSerializer.Deserialize<List<Patient>>(patientsData, serializerOptions);
                if (patients == null)
                    return;
                await dbContext.Patients.AddRangeAsync(patients);
                await dbContext.SaveChangesAsync();
            }
            if (!await dbContext.Appointments.AnyAsync())
            {
                var appointmentsData = File.ReadAllText(Path.Combine(seedingDirectory, "Appointments.json"));
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(appointmentsData, serializerOptions);
                if (appointments == null)
                    return;
                await dbContext.Appointments.AddRangeAsync(appointments);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}