namespace HealthCareApp.Entities
{
    public class Appointment : BaseEntity
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
