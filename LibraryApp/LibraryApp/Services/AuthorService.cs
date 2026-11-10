using System.Linq.Expressions;
using AutoMapper;
using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.AuthorDtos;
using LibraryApp.Entities;
using LibraryApp.Repositories;

namespace LibraryApp.Services
{
    public sealed class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<Author>> GetPageAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            Expression<Func<Author, bool>> predicate = author =>
                searchTerm == null || author.Name.Contains(searchTerm);
            var totalRecords = await _unitOfWork.Authors.CountAsync(predicate);
            var authors = await _unitOfWork.Authors.GetAsync(
                predicate,
                (pageNumber - 1) * pageSize,
                pageSize,
                author => author.Books!);

            return new PaginatedList<Author>
            {
                Data = authors,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public Task<Author?> GetByIdAsync(int id)
        {
            return _unitOfWork.Authors.GetByIdAsync(id, author => author.Books!);
        }

        public async Task<Author> CreateAsync(CreateAuthorRequest request)
        {
            var author = _mapper.Map<Author>(request);
            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();
            return author;
        }

        public async Task<ServiceResult<Author>> UpdateAsync(int id, UpdateAuthorRequest request)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, entity => entity.Books!);
            if (author is null)
            {
                return ServiceResult<Author>.Failure(ServiceErrorType.NotFound, $"Author with ID {id} not found.");
            }

            _mapper.Map(request, author);
            await _unitOfWork.Authors.UpdateAsync(author);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Author>.Success(author);
        }

        public async Task<ServiceResult<Author>> DeleteAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, entity => entity.Books!);
            if (author is null)
            {
                return ServiceResult<Author>.Failure(ServiceErrorType.NotFound, $"Author with ID {id} not found.");
            }

            await _unitOfWork.Authors.DeleteAsync(author);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Author>.Success(author);
        }
    }
}
