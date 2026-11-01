using HealthCareApp.Entities;

namespace HealthCareApp.Dtos.AppointmentDtos
{
    public class CreateAppointmentRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
