using LibraryApp.Data;
using LibraryApp.Dtos.BookDtos;
using LibraryApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public BooksController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var Books = await _dbContext.Books.Include(b => b.Author).ToListAsync();
            if (Books == null || !Books.Any())
            {
                return NotFound("No books found.");
            }
            var response = Books.Select(b => new GetBookResponse
            {
                Title = b.Title,
                ISBN = b.ISBN,
                AuthorId = b.AuthorId,
                AuthorName = b.Author.Name
            }).ToList();
            return Ok(response);
            
        }


        [HttpGet("GetBookById/{id}")]
        public async Task<IActionResult> GetBookByID(int id)
        {
            var Book = await _dbContext.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
            if (Book == null) 
            {
                return NotFound("No such book with the passed ID");
            }
            var response = new GetBookResponse
            {
                Title = Book.Title,
                ISBN = Book.ISBN,
                AuthorId = Book.AuthorId,
                AuthorName = Book.Author.Name
            };
            return Ok(response);
        }

        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook(CreateBookRequest request)
        {
            var author = await _dbContext.Authors.FindAsync(request.AuthorId);
            if (author == null)
            {
                return NotFound($"Author with ID {request.AuthorId} was not found.");
            }

            var book = new Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                AuthorId = author.Id,
                Author = author
            };

            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, new
            {
                book.Id,
                book.Title,
                book.ISBN,
                book.AuthorId
            });
        }
        [HttpPut("UpdateBook/{id}")]
        public async Task<IActionResult> UpdateBook(int id,UpdateBookRequest request)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (id <= 0)
                return BadRequest("Invalid book ID.");
            
            if (book == null)
                return NotFound($"Book with ID {id} not found.");

            book.AuthorId = request.AuthorId;
            book.Title = request.Title;
            book.ISBN = request.ISBN;

            _dbContext.Books.Update(book);

            await _dbContext.SaveChangesAsync();
            return Ok(book);

        }
        [HttpDelete("DeleteBook/{id}")]
            public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (id <= 0)
                return BadRequest("Invalid book ID.");

            if (book == null)
                return NotFound($"Book with ID {id} not found.");
            book.IsDeleted = true;

            //_dbContext.Books.Remove(book);
            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
            return Ok($"Book with ID {id} has been deleted.");
        }
       
        

    }
}
