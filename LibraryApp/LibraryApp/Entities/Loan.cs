using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
namespace LibraryApp.Entities
{
    public class Loan : BaseEntity
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int BorrowerId { get; set; }
        public Borrower? Borrower { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(14);
        public int RemainingDays => (ReturnDate - DateTime.Now).Days;
    }
}
