using AutoMapper;
using LibraryApp.Dtos.AuthorDtos;
using LibraryApp.Entities;
namespace LibraryApp.Helper
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, GetAuthorResponse>()
                .ForMember(dest => dest.BooksTitles, 
                opt => opt.MapFrom(src => src.Books != null ? src.Books.Select(book => book.Title).ToList() : new List<string>()));
            CreateMap<CreateAuthorRequest, Author>();
            CreateMap<UpdateAuthorRequest, Author>();
        }
    }
}
