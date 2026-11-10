using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.BookDtos;
using LibraryApp.Entities;

namespace LibraryApp.Services
{
    public interface IBookService
    {
        Task<PaginatedList<Book>> GetPageAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<Book?> GetByIdAsync(int id);
        Task<ServiceResult<Book>> CreateAsync(CreateBookRequest request);
        Task<ServiceResult<Book>> UpdateAsync(int id, UpdateBookRequest request);
        Task<ServiceResult<Book>> DeleteAsync(int id);
    }
}
