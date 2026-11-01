namespace HealthCareApp.Dtos.AppointmentDtos
{
    public class GetAppointmentResponse
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
