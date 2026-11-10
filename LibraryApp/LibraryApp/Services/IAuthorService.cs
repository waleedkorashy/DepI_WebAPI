using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.AuthorDtos;
using LibraryApp.Entities;

namespace LibraryApp.Services
{
    public interface IAuthorService
    {
        Task<PaginatedList<Author>> GetPageAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<Author?> GetByIdAsync(int id);
        Task<Author> CreateAsync(CreateAuthorRequest request);
        Task<ServiceResult<Author>> UpdateAsync(int id, UpdateAuthorRequest request);
        Task<ServiceResult<Author>> DeleteAsync(int id);
    }
}
