using System.Linq.Expressions;
using AutoMapper;
using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.BorrowerDtos;
using LibraryApp.Entities;
using LibraryApp.Repositories;

namespace LibraryApp.Services
{
    public sealed class BorrowerService : IBorrowerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BorrowerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<Borrower>> GetPageAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            Expression<Func<Borrower, bool>> predicate = borrower =>
                searchTerm == null || borrower.Name.Contains(searchTerm);
            var totalRecords = await _unitOfWork.Borrowers.CountAsync(predicate);
            var borrowers = await _unitOfWork.Borrowers.GetAsync(
                predicate,
                (pageNumber - 1) * pageSize,
                pageSize,
                borrower => borrower.Loans!);

            return new PaginatedList<Borrower>
            {
                Data = borrowers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public Task<Borrower?> GetByIdAsync(int id)
        {
            return _unitOfWork.Borrowers.GetByIdAsync(id, borrower => borrower.Loans!);
        }

        public async Task<Borrower> CreateAsync(CreateBorrowerRequest request)
        {
            var borrower = _mapper.Map<Borrower>(request);
            await _unitOfWork.Borrowers.AddAsync(borrower);
            await _unitOfWork.SaveChangesAsync();
            return borrower;
        }

        public async Task<ServiceResult<Borrower>> UpdateAsync(int id, UpdateBorrowerRequest request)
        {
            var borrower = await GetByIdAsync(id);
            if (borrower is null)
            {
                return ServiceResult<Borrower>.Failure(ServiceErrorType.NotFound, $"Borrower with ID {id} not found.");
            }

            _mapper.Map(request, borrower);
            await _unitOfWork.Borrowers.UpdateAsync(borrower);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Borrower>.Success(borrower);
        }

        public async Task<ServiceResult<Borrower>> DeleteAsync(int id)
        {
            var borrower = await _unitOfWork.Borrowers.GetByIdAsync(id);
            if (borrower is null)
            {
                return ServiceResult<Borrower>.Failure(ServiceErrorType.NotFound, $"Borrower with ID {id} not found.");
            }

            await _unitOfWork.Borrowers.DeleteAsync(borrower);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Borrower>.Success(borrower);
        }
    }
}
