using HealthCareApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthCareApp.Data.Configurations
{
    public class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            
            builder.HasMany(d => d.Appointments)
                   .WithOne(a => a.Doctor)
                   .HasForeignKey(a => a.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasQueryFilter(d => !d.IsDeleted);

        }
    }
}
