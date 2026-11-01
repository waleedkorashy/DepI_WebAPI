using HealthCareApp.Entities;

namespace HealthCareApp.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
