using LibraryApp.Entities;

namespace LibraryApp.Dtos.AuthorDtos
{
    public class GetAuthorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
        public ICollection<string>? BooksTitles { get; set; }
    }
}
