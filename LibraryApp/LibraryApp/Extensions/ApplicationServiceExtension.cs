using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using AutoMapper;
using LibraryApp.Helper;
using LibraryApp.Repositories;
using LibraryApp.Services;



namespace LibraryApp.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultSqlConnection"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBorrowerService, BorrowerService>();
            services.AddScoped<ILoanService, LoanService>();

            services.AddAutoMapper(cfg => {
                cfg.AddProfile<AuthorProfile>();
                cfg.AddProfile<LoanProfile>();
                cfg.AddProfile<BookProfile>();
                cfg.AddProfile<BorrowerProfile>();
            });


            return services;
        }
    }
}
