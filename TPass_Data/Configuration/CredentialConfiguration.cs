using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPass_Data.Entities;
namespace TPass_Data.Configuration;
public class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public void Configure(EntityTypeBuilder<Credential> builder)
    {
        // Negative seed IDs avoid collisions with ordinary identity-generated IDs.
        builder.HasData(
            new Credential { Id = -1, UserId = "tpass-demo-user-1", Website = "https://demo1.example",
                Username = "demo1", Password = "SamplePassword1!",
                CreatedOn = new DateTime(2026, 11, 3, 0, 0, 0, DateTimeKind.Utc) },
            new Credential { Id = -2, UserId = "tpass-demo-user-2", Website = "https://demo2.example",
                Username = "demo2", Password = "SamplePassword2!",
                CreatedOn = new DateTime(2026, 11, 3, 0, 0, 0, DateTimeKind.Utc) });
    }
}
