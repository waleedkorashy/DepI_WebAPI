using LibraryApp.Dtos.AuthorDtos;
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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger, IMapper mapper)
        {
            _authorService = authorService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            if (pageSize > 20)
            {
                return BadRequest("Page size cannot exceed 20.");
            }

            var page = await _authorService.GetPageAsync(pageNumber, pageSize, searchTerm);
            var authors = page.Data?.ToList() ?? [];

            if (authors.Count == 0)
            {
                return NotFound(searchTerm != null
                    ? "No authors found matching the search term."
                    : "No authors found.");
            }
            _logger.LogInformation($"Total authors found: {page.TotalRecords}", authors);
            var authorsResponse = _mapper.Map<List<Author>, List<GetAuthorResponse>>(authors);
            var paginatedResponse = new PaginatedList<GetAuthorResponse>
            {
                Data = authorsResponse,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = page.TotalRecords
            };
            var result = GenericResult<PaginatedList<GetAuthorResponse>>.Success(paginatedResponse);
            return Ok(result);
        }

        [HttpGet("GetAuthorById/{id}")]
        public async Task<IActionResult> GetAuthorByID(int id)
        {
            var Author = await _authorService.GetByIdAsync(id);
            if (Author == null)
            {
                return NotFound("No such author with the passed ID");
            }
            var response = _mapper.Map<Author, GetAuthorResponse>(Author);
            _logger.LogInformation($"Author found: {response.Name}", response);
            return Ok(response);
        }
        [HttpPost("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor(CreateAuthorRequest request)
        {
            var author = await _authorService.CreateAsync(request);
            _logger.LogInformation($"Author created: {author.Id} - {author.Name}", author);
            return StatusCode(StatusCodes.Status201Created, _mapper.Map<Author, GetAuthorResponse>(author));
        }

        [HttpPut("UpdateAuthor/{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorRequest request)
        {
            if (id <= 0)
                return BadRequest("Invalid author ID.");

            var result = await _authorService.UpdateAsync(id, request);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var author = result.Value!;
            _logger.LogInformation($"Author updated: {author.Id} - {author.Name}", author);
            return Ok(_mapper.Map<Author, GetAuthorResponse>(author));

        }

        [HttpDelete("DeleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid author ID.");

            var result = await _authorService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var author = result.Value!;
            _logger.LogInformation($"Author deleted: {author.Id} - {author.Name}", author);
            return Ok(_mapper.Map<Author, GetAuthorResponse>(author));
        }
    }
}
