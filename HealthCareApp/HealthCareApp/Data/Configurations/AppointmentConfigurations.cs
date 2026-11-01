using HealthCareApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthCareApp.Data.Configurations
{
    public class AppointmentConfigurations : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => new {a.DoctorId, a.PatientId });

            builder.Property(a => a.AppointmentDate)
                   .IsRequired();
            builder.HasQueryFilter(a => !a.IsDeleted);
        }
    }
}


