using LibraryApp.Data;
using LibraryApp.Dtos.AuthorDtos;
using LibraryApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public AuthorsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _dbContext.Authors.ToListAsync();
            if (authors == null || !authors.Any())
            {
                return NotFound("No authors found.");
            }
            var response = authors.Select(a => new GetAuthorResponse
            {
                Name = a.Name,
                BirthDate = a.BirthDate
            }).ToList();
            return Ok(response);
        }

        [HttpGet("GetAuthorById/{id}")]
        public async Task<IActionResult> GetAuthorByID(int id)
        {
            var Author = await _dbContext.Authors.FindAsync(id);
            if (Author == null)
            {
                return NotFound("No such author with the passed ID");
            }
            var response = new GetAuthorResponse
            {
                Name = Author.Name,
                BirthDate = Author.BirthDate
            };
            return Ok(response);
        }
        [HttpPost("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor(CreateAuthorRequest request)
        {
            var author = new Author
            {
                Name = request.Name,
                BirthDate = request.BirthDate
            };

            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, new
            {
                author.Id,
                author.Name,
                author.BirthDate
            });
        }

        [HttpPut("UpdateAuthor/{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorRequest request)
        {
            var author = await _dbContext.Authors.FindAsync(id);
            if (id <= 0)
                return BadRequest("Invalid author ID.");

            if (author == null)
                return NotFound($"Author with ID {id} not found.");

            author.Name = request.Name;
            author.BirthDate = request.BirthDate;

            _dbContext.Authors.Update(author);

            await _dbContext.SaveChangesAsync();
            return Ok(author);

        }

        [HttpDelete("DeleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _dbContext.Authors.FindAsync(id);
            if (id <= 0)
                return BadRequest("Invalid author ID.");

            if (author == null)
                return NotFound($"Author with ID {id} not found.");
            author.IsDeleted = true;

            _dbContext.Authors.Update(author);
            await _dbContext.SaveChangesAsync();
            return Ok($"Author with ID {id} has been deleted.");
        }
    }
}
