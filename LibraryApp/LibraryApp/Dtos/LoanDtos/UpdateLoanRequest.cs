namespace LibraryApp.Dtos.LoanDtos
{
    public class UpdateLoanRequest
    {
        public int BorrowerId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(14);
    }
}
