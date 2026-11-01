namespace LibraryApp.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
