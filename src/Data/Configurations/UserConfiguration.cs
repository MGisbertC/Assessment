using MGisbert.Appointments.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MGisbert.Appointments.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p=>p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(100); 
            builder.Property(p => p.Password).IsRequired().HasMaxLength(255);
            builder.Property(p => p.RoleId).IsRequired();
            builder.HasIndex(p => p.Email).IsUnique();
            builder.HasOne(p => p.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(p => p.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(p => p.Appointments)
                    .WithOne(a => a.User)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.HasData(GetUsers());
        }

        public List<User> GetUsers()
        {
            return new List<User>
            {
                new User { Id = 1, Name = "Manager", Email = "manager@email.com", Password = "password", RoleId = 1 },
                new User { Id = 3, Name = "User1", Email = "user1@email.com", Password = "password", RoleId = 2 },
            };
        }
    }
}
