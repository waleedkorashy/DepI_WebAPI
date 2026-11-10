using LibraryApp.Dtos.BookDtos;
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
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
            _logger = logger;
        }




        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            if (pageSize > 20)
            {
                return BadRequest("Page size cannot exceed 20.");
            }

            var page = await _bookService.GetPageAsync(pageNumber, pageSize, searchTerm);
            var books = page.Data?.ToList() ?? [];

            if (books.Count == 0)
            {
                return NotFound(searchTerm != null
                    ? "No books found matching the search term."
                    : "No books found for the specified page.");
            }
            _logger.LogInformation($"Total books found: {page.TotalRecords}", books);

            var booksResponse = _mapper.Map<List<Book>, List<GetBookResponse>>(books);
            var paginatedResponse = new PaginatedList<GetBookResponse>
            {
                Data = booksResponse,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = page.TotalRecords
            };
            var result = GenericResult<PaginatedList<GetBookResponse>>.Success(paginatedResponse);
            _logger.LogInformation($"Returning paginated response for books: Page {pageNumber}, Size {pageSize}", paginatedResponse);
            return Ok(result);
            
        }






        [HttpGet("GetBookById/{id}")]
        public async Task<IActionResult> GetBookByID(int id)
        {
            var Book = await _bookService.GetByIdAsync(id);

            if (Book == null)
            {
                return NotFound("No such book with the passed ID");
            }

            var response = _mapper.Map<Book, GetBookResponse>(Book);
            _logger.LogInformation($"Book found: {response.Title}", response);
            return Ok(response);
        }





        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook(CreateBookRequest request)
        {
            var result = await _bookService.CreateAsync(request);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var book = result.Value!;
            _logger.LogInformation($"Book created: {book.Id} - {book.Title}", book);
            return StatusCode(StatusCodes.Status201Created, _mapper.Map<Book, GetBookResponse>(book));
        }




        [HttpPut("UpdateBook/{id}")]
        public async Task<IActionResult> UpdateBook(int id,UpdateBookRequest request)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");

            var result = await _bookService.UpdateAsync(id, request);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            var book = result.Value!;
            _logger.LogInformation($"Book updated: {book.Id} - {book.Title}", book);
            return Ok(_mapper.Map<Book, GetBookResponse>(book));

        }






        [HttpDelete("DeleteBook/{id}")]
            public async Task<IActionResult> DeleteBook(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");

            var result = await _bookService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return result.ErrorType == ServiceErrorType.Conflict
                    ? BadRequest(result.Error)
                    : NotFound(result.Error);
            }

            var book = result.Value!;
            _logger.LogInformation($"Book deleted: {book.Id} - {book.Title}", book);
            return Ok(_mapper.Map<Book, GetBookResponse>(book));
        }
       
        

    }
}
