namespace LibraryApp.Dtos.AuthorDtos
{
    public class CreateAuthorRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
    }
}
