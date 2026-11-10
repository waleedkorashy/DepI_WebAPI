using LibraryApp.Entities;

namespace LibraryApp.Dtos.LoanDtos
{
    public class GetLoanResponse
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int BorrowerId { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public string BorrowerEmail { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(14);
        public int RemainingDays => (ReturnDate - DateTime.Now).Days;
    }
}
