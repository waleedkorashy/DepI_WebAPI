using AutoMapper;
using LibraryApp.Dtos.BookDtos;
using LibraryApp.Entities;

namespace LibraryApp.Helper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();
            CreateMap<Book, GetBookResponse>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : string.Empty))
                .ForMember(dest => dest.LoanIds, opt => opt.MapFrom(src => src.Loans != null ? src.Loans.Select(loan => loan.Id).ToList() : new List<int>()));        }
    }
}
