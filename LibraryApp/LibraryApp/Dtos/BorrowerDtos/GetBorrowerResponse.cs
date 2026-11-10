using LibraryApp.Entities;

namespace LibraryApp.Dtos.BorrowerDtos
{
    public class GetBorrowerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateOnly MembershipDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public ICollection<int> LoansIds { get; set; } = [];
    }
}
