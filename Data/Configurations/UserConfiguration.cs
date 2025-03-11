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
            builder.Property(p => p.Email).HasMaxLength(100); 
            builder.Property(p => p.RoleId).IsRequired();
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
                new User { Id = 1, Name = "Manager", Email = "manager@email.com", RoleId = 1 },
                new User { Id = 2, Name = "Manager2", Email = "manager2@email.com", RoleId = 1 },
                new User { Id = 3, Name = "User1", Email = "user1@email.com", RoleId = 2 },
                new User { Id = 4, Name = "User2", Email = "user2@email.com", RoleId = 2 },
            };
        }
    }
}
