namespace HealthCareApp.Dtos.PatientDtos
{
    public class UpdatePatientRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
