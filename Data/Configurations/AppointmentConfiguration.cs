using MGisbert.Appointments.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGisbert.Appointments.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.Property(p=>p.Title).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Description).HasMaxLength(200);
            builder.HasOne(r => r.User)
                  .WithMany(u => u.Appointments)
                  .HasForeignKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
