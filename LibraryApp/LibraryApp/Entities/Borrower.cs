using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Entities
{
    public class Borrower : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateOnly MembershipDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public ICollection<Loan>? Loans { get; set; }
    }
}
