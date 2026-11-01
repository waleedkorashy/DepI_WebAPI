namespace HealthCareApp.Dtos.AppointmentDtos
{
    public class UpdateAppointmentRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
