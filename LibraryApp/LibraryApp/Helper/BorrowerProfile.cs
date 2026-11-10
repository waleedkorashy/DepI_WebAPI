using LibraryApp.Dtos.BorrowerDtos;
using LibraryApp.Entities;
using AutoMapper;


namespace LibraryApp.Helper
{
    public class BorrowerProfile : Profile
    {
        public BorrowerProfile()
        {
            CreateMap<Borrower, GetBorrowerResponse>()
                .ForMember(dest => dest.LoansIds, opt => opt.MapFrom(src => src.Loans != null ? src.Loans.Select(loan => loan.Id).ToList() : new List<int>()));
            CreateMap<UpdateBorrowerRequest, Borrower>();
            CreateMap<CreateBorrowerRequest, Borrower>();
        }
    }
}
