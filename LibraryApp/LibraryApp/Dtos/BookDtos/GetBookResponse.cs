namespace LibraryApp.Dtos.BookDtos
{
    public class GetBookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? ISBN { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public List<int> LoanIds { get; set; } = new List<int>();
    }
}
