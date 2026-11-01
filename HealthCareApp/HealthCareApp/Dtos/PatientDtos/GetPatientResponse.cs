namespace HealthCareApp.Dtos.PatientDtos
{
    public class GetPatientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
