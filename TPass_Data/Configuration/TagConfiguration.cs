using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPass_Data.Entities;
namespace TPass_Data.Configuration;
public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasQueryFilter(t => !t.IsDeleted);
        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        builder.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasData(
            new Tag { Id = -1, Name = "Work", UserId = "tpass-demo-user-1" },
            new Tag { Id = -2, Name = "Personal", UserId = "tpass-demo-user-2" });
    }
}

