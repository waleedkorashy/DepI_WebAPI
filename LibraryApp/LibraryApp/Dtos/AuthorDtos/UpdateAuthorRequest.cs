namespace LibraryApp.Dtos.AuthorDtos
{
    public class UpdateAuthorRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
    }
}
