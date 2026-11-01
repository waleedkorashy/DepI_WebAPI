using HealthCareApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthCareApp.Data.Configurations
{
    public class PatientConfigurations : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(p => p.Appointments)
                   .WithOne(a => a.Patient)
                   .HasForeignKey(a => a.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasQueryFilter(p => !p.IsDeleted);

        }
    }
}
