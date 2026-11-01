
using LibraryApp.Data;
using LibraryApp.Extensions;
using Microsoft.EntityFrameworkCore;


namespace LibraryApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddServices(builder.Configuration);

            var app = builder.Build();
            #region Seeding

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                using var dbcontext = services.GetRequiredService<ApplicationDbContext>();
                await dbcontext.Database.MigrateAsync();
                await LibraryContextSeeding.SeedAsync(dbcontext);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError(ex, ex.Message);
            }
            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
