using HealthCareApp.Entities;

namespace HealthCareApp.Dtos.DoctorDtos
{
    public class GetDoctorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
