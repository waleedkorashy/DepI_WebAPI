using HealthCareApp.Entities;

namespace HealthCareApp.Entities
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
