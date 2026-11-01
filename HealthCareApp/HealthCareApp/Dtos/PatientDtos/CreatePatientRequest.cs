namespace HealthCareApp.Dtos.PatientDtos
{
    public class CreatePatientRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
