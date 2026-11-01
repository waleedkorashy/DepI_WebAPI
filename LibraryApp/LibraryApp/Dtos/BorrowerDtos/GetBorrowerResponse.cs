namespace LibraryApp.Dtos.BorrowerDtos
{
    public class GetBorrowerResponse
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateOnly MembershipDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
