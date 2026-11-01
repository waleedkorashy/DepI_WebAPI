using HealthCareApp.Entities;

namespace HealthCareApp.Dtos.DoctorDtos
{
    public class CreateDoctorRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;

    }
}
