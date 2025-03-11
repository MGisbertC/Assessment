using MGisbert.Appointments.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGisbert.Appointments.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(p=>p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Description).HasMaxLength(200);
            builder.HasMany(r => r.Users)
                  .WithOne(u => u.Role)
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
            builder.HasData(GetRoles());
        }

        public List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = "Manager", Description = "Manager" },
                new Role { Id = 2, Name = "User", Description = "User" }
            };
        }
    }
}
