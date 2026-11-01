using LibraryApp.Data;
using LibraryApp.Dtos.BorrowerDtos;
using LibraryApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowersController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        public BorrowersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet("GetAllBorrowers")]
        public async Task<IActionResult> GetAllBorrowers()
        {
            var borrowers = await _dbContext.Borrowers.ToListAsync();
            if (borrowers == null || !borrowers.Any())
            {
                return NotFound("No borrowers found.");
            }
            var response = borrowers.Select(b => new GetBorrowerResponse
            {
                Name = b.Name,
                Email = b.Email,
                MembershipDate = b.MembershipDate
            }).ToList();
            return Ok(response);
        }

        [HttpGet("GetBorrowerById/{id}")]
        public async Task<IActionResult> GetBorrowerById(int id)
        {
            var borrower = await _dbContext.Borrowers.FindAsync(id);
            if (borrower == null)
            {
                return NotFound("No such borrower with the passed ID");
            }
            var response = new GetBorrowerResponse
            {
                Name = borrower.Name,
                Email = borrower.Email,
                MembershipDate = borrower.MembershipDate
            };
            return Ok(response);
        }

        [HttpPost("CreateBorrower")]
        public async Task<IActionResult> CreateBorrower(CreateBorrowerRequest request)
        {
            var borrower = new Borrower
            {
                Name = request.Name,
                Email = request.Email,
                MembershipDate = request.MembershipDate
            };
            await _dbContext.Borrowers.AddAsync(borrower);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, new
            {
                borrower.Id,
                borrower.Email,
                borrower.MembershipDate
            });
        }
        [HttpPut("UpdateBorrower/{id}")]
        public async Task<IActionResult> UpdateBorrower(int id, UpdateBorrowerRequest request)
        {
            var borrower = await _dbContext.Borrowers.FindAsync(id);
            if (id <= 0)
                return BadRequest("Invalid borrower ID.");

            if (borrower == null)
                return NotFound($"Borrower with ID {id} not found.");

            borrower.Name = request.Name;
            borrower.Email = request.Email;
            borrower.MembershipDate = request.MembershipDate;

            _dbContext.Borrowers.Update(borrower);

            await _dbContext.SaveChangesAsync();
            return Ok(borrower);
        }
        [HttpDelete("DeleteBorrower/{id}")]
        public async Task<IActionResult> DeleteBorrower(int id)
        {
            var borrower = await _dbContext.Borrowers.FindAsync(id);
            if (id <= 0)
                return BadRequest("Invalid borrower ID.");

            if (borrower == null)
                return NotFound($"Borrower with ID {id} not found.");
            borrower.IsDeleted = true;

            _dbContext.Borrowers.Update(borrower);
            await _dbContext.SaveChangesAsync();
            return Ok($"Borrower with ID {id} has been deleted.");
        }
    }
}
