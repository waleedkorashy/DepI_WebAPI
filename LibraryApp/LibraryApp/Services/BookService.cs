using System.Linq.Expressions;
using AutoMapper;
using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.BookDtos;
using LibraryApp.Entities;
using LibraryApp.Repositories;

namespace LibraryApp.Services
{
    public sealed class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<Book>> GetPageAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            Expression<Func<Book, bool>> predicate = book =>
                searchTerm == null || book.Title.Contains(searchTerm) ||
                (book.Author != null && book.Author.Name.Contains(searchTerm));
            var totalRecords = await _unitOfWork.Books.CountAsync(predicate);
            var books = await _unitOfWork.Books.GetAsync(
                predicate,
                (pageNumber - 1) * pageSize,
                pageSize,
                book => book.Author!,
                book => book.Loans!);

            return new PaginatedList<Book>
            {
                Data = books,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            return _unitOfWork.Books.GetByIdAsync(id, book => book.Author!, book => book.Loans!);
        }

        public async Task<ServiceResult<Book>> CreateAsync(CreateBookRequest request)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId);
            if (author is null)
            {
                return ServiceResult<Book>.Failure(
                    ServiceErrorType.NotFound,
                    $"Author with ID {request.AuthorId} was not found.");
            }

            var book = _mapper.Map<Book>(request);
            book.Author = author;
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Book>.Success(book);
        }

        public async Task<ServiceResult<Book>> UpdateAsync(int id, UpdateBookRequest request)
        {
            var book = await GetByIdAsync(id);
            if (book is null)
            {
                return ServiceResult<Book>.Failure(ServiceErrorType.NotFound, $"Book with ID {id} not found.");
            }

            var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId);
            if (author is null)
            {
                return ServiceResult<Book>.Failure(
                    ServiceErrorType.NotFound,
                    $"Author with ID {request.AuthorId} was not found.");
            }

            _mapper.Map(request, book);
            book.Author = author;
            await _unitOfWork.Books.UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Book>.Success(book);
        }

        public async Task<ServiceResult<Book>> DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            if (book is null)
            {
                return ServiceResult<Book>.Failure(ServiceErrorType.NotFound, $"Book with ID {id} not found.");
            }

            if (book.Loans?.Any(loan => !loan.IsDeleted) == true)
            {
                return ServiceResult<Book>.Failure(ServiceErrorType.Conflict, "The book has active loans.");
            }

            await _unitOfWork.Books.DeleteAsync(book);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Book>.Success(book);
        }
    }
}
