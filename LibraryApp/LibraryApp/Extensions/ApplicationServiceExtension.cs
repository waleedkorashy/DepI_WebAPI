using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using AutoMapper;


namespace LibraryApp.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultSqlConnection"));
            });

            return services;
        }
    }
}
