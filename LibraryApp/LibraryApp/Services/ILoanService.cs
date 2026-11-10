using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.LoanDtos;
using LibraryApp.Entities;

namespace LibraryApp.Services
{
    public interface ILoanService
    {
        Task<PaginatedList<Loan>> GetPageAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<IReadOnlyList<Loan>> GetByBookIdAsync(int bookId);
        Task<IReadOnlyList<Loan>> GetByBorrowerIdAsync(int borrowerId);
        Task<Loan?> GetByIdAsync(int id);
        Task<ServiceResult<Loan>> CreateAsync(CreateLoanRequest request);
        Task<ServiceResult<Loan>> UpdateAsync(int id, UpdateLoanRequest request);
        Task<ServiceResult<Loan>> DeleteAsync(int id);
    }
}
