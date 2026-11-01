using HealthCareApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

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
