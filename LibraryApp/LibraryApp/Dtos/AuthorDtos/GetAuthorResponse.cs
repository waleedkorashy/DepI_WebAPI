namespace LibraryApp.Dtos.AuthorDtos
{
    public class GetAuthorResponse
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
    }
}
