namespace LibraryApp.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public int? ISBN { get; set; }
        public int AuthorId { get; set; }

        public Author? Author { get; set; }
        public ICollection<Loan>? Loans { get; set; }

    }
}
