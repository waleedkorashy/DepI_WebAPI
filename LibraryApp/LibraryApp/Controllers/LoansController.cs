using LibraryApp.Data;
using LibraryApp.Dtos.LoanDtos;
using LibraryApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public LoansController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAllLoans")]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _dbContext.Loans.ToListAsync();
            if (loans == null || !loans.Any())
            {
                return NotFound("No loans found.");
            }
            var response = loans.Select(l => new GetLoanResponse
            {
                BookId = l.BookId,
                BorrowerId = l.BorrowerId,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate
            }).ToList();
            return Ok(response);
        }

        [HttpGet("{bookId:int}/Loans")]
        public async Task<IActionResult> GetLoansByBookId(int bookId)
        {
            var loans = await _dbContext.Loans.Where(l => l.BookId == bookId).ToListAsync();
            if (loans == null || !loans.Any())
            {
                return NotFound("No loans found for the specified book.");
            }
            var response = loans.Select(l => new GetLoanResponse
            {
                BookId = l.BookId,
                BorrowerId = l.BorrowerId,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate
            }).ToList();
            return Ok(response);
        }

        [HttpGet("{loanId:int}")]
        public async Task<IActionResult> GetLoanById(int loanId)
        {
            var loan = await _dbContext.Loans.FindAsync(loanId);
            if (loan == null)
            {
                return NotFound("Loan not found.");
            }
            return Ok(loan);
        }

        [HttpPost("{bookId:int}/Loans")]
        public async Task<IActionResult> CreateLoan(int bookId, CreateLoanRequest request)
        {
            var book = await _dbContext.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound($"Book with ID {bookId} was not found.");
            }

            var borrower = await _dbContext.Borrowers.FindAsync(request.BorrowerId);
            if (borrower == null)
            {
                return NotFound($"Borrower with ID {request.BorrowerId} was not found.");
            }

            var loan = new Loan
            {
                BookId = book.Id,
                BorrowerId = borrower.Id,
                LoanDate = request.LoanDate,
                ReturnDate = request.ReturnDate
            };

            await _dbContext.Loans.AddAsync(loan);
            await _dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, new
            {
                loan.Id,
                loan.BookId,
                loan.BorrowerId,
                loan.LoanDate,
                loan.ReturnDate
            });

        }
        [HttpPut("{loanId:int}")]
        public async Task<IActionResult> UpdateLoan(int loanId, UpdateLoanRequest request)
        {
            var loan = await _dbContext.Loans.FindAsync(loanId);
            if (loan == null)
            {
                return NotFound("Loan not found.");
            }

            loan.BorrowerId = request.BorrowerId;
            loan.LoanDate = request.LoanDate;
            loan.ReturnDate = request.ReturnDate;

            await _dbContext.SaveChangesAsync();

            return Ok(loan);
        }
        [HttpDelete("{loanId:int}")]
        public async Task<IActionResult> DeleteLoan(int loanId)
        {
            var loan = await _dbContext.Loans.FindAsync(loanId);
            if (loan == null)
            {
                return NotFound("Loan not found.");
            }

            loan.IsDeleted = true;
            _dbContext.Loans.Update(loan);

            await _dbContext.SaveChangesAsync();
            return Ok($"Loan {loanId} deleted successfully.");
        }
    }
}
