namespace LibraryApp.Dtos.BookDtos
{
    public class CreateBookRequest
    {
        public string Title { get; set; } = string.Empty;
        public int? ISBN { get; set; }
        public int AuthorId { get; set; }

    }
}
