using LibraryApp.Dtos.BorrowerDtos;
using LibraryApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LibraryApp.Common.Results;
using LibraryApp.Common.Pagination;
using LibraryApp.Services;
namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowersController : ControllerBase
    {
        private readonly IBorrowerService _borrowerService;
        private readonly IMapper _mapper;
        private readonly ILogger<BorrowersController> _logger;

        public BorrowersController(IBorrowerService borrowerService, ILogger<BorrowersController> logger, IMapper mapper)
        {
            _borrowerService = borrowerService;
            _logger = logger;
            _mapper = mapper;
        }
        
        [HttpGet("GetAllBorrowers")]
        public async Task<IActionResult> GetAllBorrowers(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            if (pageSize > 20)
            {
                return BadRequest("Page size cannot exceed 20.");
            }

            var page = await _borrowerService.GetPageAsync(pageNumber, pageSize, searchTerm);
            var borrowers = page.Data?.ToList() ?? [];

            if (borrowers.Count == 0)
            {
                return NotFound(searchTerm != null
                    ? "No borrowers found matching the search term."
                    : "No borrowers found.");
            }
            _logger.LogInformation($"Total borrowers found: {page.TotalRecords}", borrowers);

            var borrowerResponse = _mapper.Map<List<Borrower>, List<GetBorrowerResponse>>(borrowers);
            var paginatedResponse = new PaginatedList<GetBorrowerResponse>
            {
                Data = borrowerResponse,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = page.TotalRecords
            };
            var result = GenericResult<PaginatedList<GetBorrowerResponse>>.Success(paginatedResponse);
            return Ok(result);
        }

        [HttpGet("GetBorrowerById/{id}")]
        public async Task<IActionResult> GetBorrowerById(int id)
        {
            var borrower = await _borrowerService.GetByIdAsync(id);
            if (borrower == null)
            {
                return NotFound("No such borrower with the passed ID");
            }
            _logger.LogInformation($"Borrower found: {borrower.Name}", borrower);
            var response = _mapper.Map<Borrower, GetBorrowerResponse>(borrower);
            return Ok(response);
        }


        [HttpPost("CreateBorrower")]
        public async Task<IActionResult> CreateBorrower(CreateBorrowerRequest request)
        {

            var borrower = await _borrowerService.CreateAsync(request);
            _logger.LogInformation($"Borrower created: {borrower.Name}", borrower);
            return StatusCode(StatusCodes.Status201Created, _mapper.Map<Borrower, GetBorrowerResponse>(borrower));
        }


        [HttpPut("UpdateBorrower/{id}")]
        public async Task<IActionResult> UpdateBorrower(int id, UpdateBorrowerRequest request)
        {
            if (id <= 0)
                return BadRequest("Invalid borrower ID.");

            var result = await _borrowerService.UpdateAsync(id, request);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var borrower = result.Value!;
            _logger.LogInformation($"Borrower updated: {borrower.Name}", borrower);
            return Ok(_mapper.Map<Borrower, GetBorrowerResponse>(borrower));
        }


        [HttpDelete("DeleteBorrower/{id}")]
        public async Task<IActionResult> DeleteBorrower(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid borrower ID.");

            var result = await _borrowerService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var borrower = result.Value!;
            _logger.LogInformation($"Borrower deleted: {borrower.Name}", borrower);
            return Ok(_mapper.Map<Borrower, GetBorrowerResponse>(borrower));
        }
    }
}
