using LibraryApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class LibraryContextSeeding
    {
        public async static Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Authors.AnyAsync())
            {
                var AuthorsData = File.ReadAllText("C:\\Users\\Waleed\\source\\repos\\DEPI_WebAPI\\LibraryApp\\LibraryApp\\Data\\Seeding\\Authors.json");
                var authors = JsonSerializer.Deserialize<List<Author>>(AuthorsData);

                if (authors == null)
                    return;

                await dbContext.Authors.AddRangeAsync(authors);
                await dbContext.SaveChangesAsync();

            }
            if (!await dbContext.Borrowers.AnyAsync())
            {
                var BorrowersData = File.ReadAllText("C:\\Users\\Waleed\\source\\repos\\DEPI_WebAPI\\LibraryApp\\LibraryApp\\Data\\Seeding\\Borrowers.json");
                var borrowers = JsonSerializer.Deserialize<List<Borrower>>(BorrowersData);
                if (borrowers == null)
                    return;
                await dbContext.Borrowers.AddRangeAsync(borrowers);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
