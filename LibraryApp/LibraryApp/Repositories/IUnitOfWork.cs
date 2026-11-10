using LibraryApp.Entities;

namespace LibraryApp.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<Author> Authors { get; }
        IGenericRepository<Book> Books { get; }
        IGenericRepository<Borrower> Borrowers { get; }
        IGenericRepository<Loan> Loans { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
