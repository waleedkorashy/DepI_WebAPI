using LibraryApp.Data;
using LibraryApp.Entities;

namespace LibraryApp.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Authors = new GenericRepository<Author>(_dbContext);
            Books = new GenericRepository<Book>(_dbContext);
            Borrowers = new GenericRepository<Borrower>(_dbContext);
            Loans = new GenericRepository<Loan>(_dbContext);
        }

        public IGenericRepository<Author> Authors { get; }
        public IGenericRepository<Book> Books { get; }
        public IGenericRepository<Borrower> Borrowers { get; }
        public IGenericRepository<Loan> Loans { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
