namespace LibraryApp.Dtos.AuthorDtos
{
    public class CreateAuthorRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
    }
}
