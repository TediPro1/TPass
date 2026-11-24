using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPass_Data.Entities;
namespace TPass_Data.Configuration;
public class CredentialTagConfiguration : IEntityTypeConfiguration<CredentialTag>
{
    public void Configure(EntityTypeBuilder<CredentialTag> builder)
    {
        builder.HasQueryFilter(ct => !ct.Tag.IsDeleted);
        builder.HasKey(ct => new { ct.CredentialId, ct.TagId });
        builder.HasOne(ct => ct.Credential).WithMany(c => c.CredentialTags).HasForeignKey(ct => ct.CredentialId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ct => ct.Tag).WithMany(t => t.CredentialTags).HasForeignKey(ct => ct.TagId);
        builder.HasData(
            new CredentialTag { CredentialId = -1, TagId = -1 },
            new CredentialTag { CredentialId = -2, TagId = -2 });
    }
}

