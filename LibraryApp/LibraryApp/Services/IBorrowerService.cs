using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.BorrowerDtos;
using LibraryApp.Entities;

namespace LibraryApp.Services
{
    public interface IBorrowerService
    {
        Task<PaginatedList<Borrower>> GetPageAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<Borrower?> GetByIdAsync(int id);
        Task<Borrower> CreateAsync(CreateBorrowerRequest request);
        Task<ServiceResult<Borrower>> UpdateAsync(int id, UpdateBorrowerRequest request);
        Task<ServiceResult<Borrower>> DeleteAsync(int id);
    }
}
