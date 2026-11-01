using LibraryApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Data.Configurations
{
    public class BorrowerConfigurations : IEntityTypeConfiguration<Borrower>
    {
        public void Configure(EntityTypeBuilder<Borrower> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(b => b.Loans)
                    .WithOne(l => l.Borrower)
                    .HasForeignKey(l => l.BorrowerId);

            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
