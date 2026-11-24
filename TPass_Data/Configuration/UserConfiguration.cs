using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPass_Data.Entities;

namespace TPass_Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Fixed hash of DemoPass123!: do not generate hashes or timestamps during model creation.
        const string passwordHash = "AQAAAAIAAYagAAAAEAHIEnsGSrP9TsBsJTygruVclvABgP0c0ZznIUGgAk9qA2jMbv42ydGAHbvBbC2T4A==";
        builder.HasData(
            new User
            {
                Id = "tpass-demo-user-1", UserName = "demo1@tpass.example",
                NormalizedUserName = "DEMO1@TPASS.EXAMPLE", Email = "demo1@tpass.example",
                NormalizedEmail = "DEMO1@TPASS.EXAMPLE", EmailConfirmed = true,
                PasswordHash = passwordHash, SecurityStamp = "tpass-demo-security-1",
                ConcurrencyStamp = "tpass-demo-concurrency-1", DisplayName = "Demo User 1",
                CreatedAt = new DateTime(2026, 11, 3, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = "tpass-demo-user-2", UserName = "demo2@tpass.example",
                NormalizedUserName = "DEMO2@TPASS.EXAMPLE", Email = "demo2@tpass.example",
                NormalizedEmail = "DEMO2@TPASS.EXAMPLE", EmailConfirmed = true,
                PasswordHash = passwordHash, SecurityStamp = "tpass-demo-security-2",
                ConcurrencyStamp = "tpass-demo-concurrency-2", DisplayName = "Demo User 2",
                CreatedAt = new DateTime(2026, 11, 3, 0, 0, 0, DateTimeKind.Utc)
            });
    }
}
