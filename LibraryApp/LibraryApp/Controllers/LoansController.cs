using AutoMapper;
using LibraryApp.Common.Pagination;
using LibraryApp.Common.Results;
using LibraryApp.Dtos.LoanDtos;
using LibraryApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Services;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IMapper _mapper;
        private readonly ILogger<LoansController> _logger;

        public LoansController(ILoanService loanService, IMapper mapper, ILogger<LoansController> logger)
        {
            _loanService = loanService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAllLoans")]
        public async Task<IActionResult> GetAllLoans(int pageNumber = 1, int pageSize = 10 , string? searchTerm = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            if (pageSize > 20)
            {
                return BadRequest("Page size cannot exceed 20.");
            }

            var page = await _loanService.GetPageAsync(pageNumber, pageSize, searchTerm);
            var loans = page.Data?.ToList() ?? [];

            if (loans.Count == 0)
            {
                return NotFound("No loans found for the specified page.");
            }

            var response = _mapper.Map<List<Loan>, List<GetLoanResponse>>(loans);
            var paginatedResponse = new PaginatedList<GetLoanResponse>
            {
                Data = response,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = page.TotalRecords
            };
            var result = GenericResult<PaginatedList<GetLoanResponse>>.Success(paginatedResponse);
            _logger.LogInformation($"Total loans found: {page.TotalRecords}", response);
            return Ok(result);
        }

        [HttpGet("{bookId:int}/BookLoans")]
        public async Task<IActionResult> GetLoansByBookId(int bookId)
        {
            var loans = await _loanService.GetByBookIdAsync(bookId);
            if (loans == null || !loans.Any())
            {
                return NotFound("No loans found for the specified book.");
            }
            var response = _mapper.Map<List<Loan>, List<GetLoanResponse>>(loans.ToList());
            _logger.LogInformation($"Total loans found for book ID {bookId}: {response.Count}", response);
            return Ok(response);
        }
        [HttpGet("{borrowerId:int}/BorrowerLoans")]
        public async Task<IActionResult> GetLoansByBorrowerId(int borrowerId)
        {
            var loans = await _loanService.GetByBorrowerIdAsync(borrowerId);
            if (loans == null || !loans.Any())
            {
                return NotFound("No loans found for the specified borrower.");
            }
            var response = _mapper.Map<List<Loan>, List<GetLoanResponse>>(loans.ToList());
            _logger.LogInformation($"Total loans found for borrower ID {borrowerId}: {response.Count}", response);
            return Ok(response);
        }

        [HttpGet("{loanId:int}/Loan")]
        public async Task<IActionResult> GetLoanById(int loanId)
        {
            var loan = await _loanService.GetByIdAsync(loanId);
            if (loan == null)
            {
                return NotFound("Loan not found.");
            }
            var response = _mapper.Map<Loan, GetLoanResponse>(loan);
            _logger.LogInformation($"Loan found: {response.Id}", response);
            return Ok(response);
        }

        [HttpPost("CreateLoan")]
        public async Task<IActionResult> CreateLoan(CreateLoanRequest request)
        {
            var result = await _loanService.CreateAsync(request);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var loan = result.Value!;
            _logger.LogInformation($"Loan created: {loan.Id} for Book ID {loan.BookId} and Borrower ID {loan    .BorrowerId}", loan);

            return StatusCode(StatusCodes.Status201Created, _mapper.Map<Loan, GetLoanResponse>(loan));

        }
        [HttpPut("{loanId:int}/UpdateLoan")]
        public async Task<IActionResult> UpdateLoan(int loanId, UpdateLoanRequest request)
        {
            var result = await _loanService.UpdateAsync(loanId, request);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var loan = result.Value!;
            _logger.LogInformation($"Loan updated: {loan.Id}", loan);

            return Ok(_mapper.Map<Loan, GetLoanResponse>(loan));
        }
        [HttpDelete("{loanId:int}/DeleteLoan")]
        public async Task<IActionResult> DeleteLoan(int loanId)
        {
            var result = await _loanService.DeleteAsync(loanId);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var loan = result.Value!;
            _logger.LogInformation($"Loan deleted: {loan.Id}", loan);

            return Ok(_mapper.Map<Loan, GetLoanResponse>(loan));
        }
    }
}
