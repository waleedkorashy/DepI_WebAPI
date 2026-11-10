using LibraryApp.Dtos.LoanDtos;
using LibraryApp.Entities;
using AutoMapper;

namespace LibraryApp.Helper
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<CreateLoanRequest, Loan>();
            CreateMap<UpdateLoanRequest, Loan>();
            CreateMap<Loan, GetLoanResponse>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty))
                .ForMember(dest => dest.BorrowerName, opt => opt.MapFrom(src => src.Borrower != null ? src.Borrower.Name : string.Empty))
                .ForMember(dest => dest.BorrowerEmail, opt => opt.MapFrom(src => src.Borrower != null ? src.Borrower.Email : string.Empty));
        }
    }
}
