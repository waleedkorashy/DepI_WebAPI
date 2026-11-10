using System.Linq.Expressions;
using AutoMapper;
using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.LoanDtos;
using LibraryApp.Entities;
using LibraryApp.Repositories;

namespace LibraryApp.Services
{
    public sealed class LoanService : ILoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<Loan>> GetPageAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            Expression<Func<Loan, bool>> predicate = loan =>
                searchTerm == null ||
                (loan.Book != null && loan.Book.Title.Contains(searchTerm)) ||
                (loan.Borrower != null && loan.Borrower.Name.Contains(searchTerm));
            var totalRecords = await _unitOfWork.Loans.CountAsync(predicate);
            var loans = await _unitOfWork.Loans.GetAsync(
                predicate,
                (pageNumber - 1) * pageSize,
                pageSize,
                loan => loan.Book!,
                loan => loan.Borrower!);

            return new PaginatedList<Loan>
            {
                Data = loans,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public Task<IReadOnlyList<Loan>> GetByBookIdAsync(int bookId)
        {
            return _unitOfWork.Loans.GetAsync(
                loan => loan.BookId == bookId,
                0,
                null,
                loan => loan.Book!,
                loan => loan.Borrower!);
        }

        public Task<IReadOnlyList<Loan>> GetByBorrowerIdAsync(int borrowerId)
        {
            return _unitOfWork.Loans.GetAsync(
                loan => loan.BorrowerId == borrowerId,
                0,
                null,
                loan => loan.Book!,
                loan => loan.Borrower!);
        }

        public Task<Loan?> GetByIdAsync(int id)
        {
            return _unitOfWork.Loans.GetByIdAsync(id, loan => loan.Book!, loan => loan.Borrower!);
        }

        public async Task<ServiceResult<Loan>> CreateAsync(CreateLoanRequest request)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book is null)
            {
                return ServiceResult<Loan>.Failure(
                    ServiceErrorType.NotFound,
                    $"Book with ID {request.BookId} was not found.");
            }

            var borrower = await _unitOfWork.Borrowers.GetByIdAsync(request.BorrowerId);
            if (borrower is null)
            {
                return ServiceResult<Loan>.Failure(
                    ServiceErrorType.NotFound,
                    $"Borrower with ID {request.BorrowerId} was not found.");
            }

            var loan = _mapper.Map<Loan>(request);
            loan.ReturnDate = request.ReturnDate ?? DateTime.UtcNow.AddDays(14);
            loan.Book = book;
            loan.Borrower = borrower;
            await _unitOfWork.Loans.AddAsync(loan);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Loan>.Success(loan);
        }

        public async Task<ServiceResult<Loan>> UpdateAsync(int id, UpdateLoanRequest request)
        {
            var loan = await GetByIdAsync(id);
            if (loan is null)
            {
                return ServiceResult<Loan>.Failure(ServiceErrorType.NotFound, "Loan not found.");
            }

            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
            if (book is null)
            {
                return ServiceResult<Loan>.Failure(
                    ServiceErrorType.NotFound,
                    $"Book with ID {request.BookId} was not found.");
            }

            var borrower = await _unitOfWork.Borrowers.GetByIdAsync(request.BorrowerId);
            if (borrower is null)
            {
                return ServiceResult<Loan>.Failure(
                    ServiceErrorType.NotFound,
                    $"Borrower with ID {request.BorrowerId} was not found.");
            }

            _mapper.Map(request, loan);
            loan.Book = book;
            loan.Borrower = borrower;
            await _unitOfWork.Loans.UpdateAsync(loan);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Loan>.Success(loan);
        }

        public async Task<ServiceResult<Loan>> DeleteAsync(int id)
        {
            var loan = await GetByIdAsync(id);
            if (loan is null)
            {
                return ServiceResult<Loan>.Failure(ServiceErrorType.NotFound, "Loan not found.");
            }

            await _unitOfWork.Loans.DeleteAsync(loan);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Loan>.Success(loan);
        }
    }
}
