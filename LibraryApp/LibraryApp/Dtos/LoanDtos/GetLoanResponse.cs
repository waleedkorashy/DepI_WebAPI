using LibraryApp.Entities;

namespace LibraryApp.Dtos.LoanDtos
{
    public class GetLoanResponse
    {
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(14);
    }
}
