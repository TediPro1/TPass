using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TPass_Data.Entities;

namespace TPass_Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<CredentialTag> CredentialTags { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Tag>().Property(t => t.Name).HasMaxLength(50).IsRequired();
            // Avoid multiple SQL Server cascade paths through the join table.
            builder.Entity<Tag>().HasOne(t => t.User).WithMany()
                .HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<CredentialTag>().HasKey(ct => new { ct.CredentialId, ct.TagId });
            builder.Entity<CredentialTag>().HasOne(ct => ct.Credential).WithMany(c => c.CredentialTags)
                .HasForeignKey(ct => ct.CredentialId);
            builder.Entity<CredentialTag>().HasOne(ct => ct.Tag).WithMany(t => t.CredentialTags)
                .HasForeignKey(ct => ct.TagId);
        }
        public DbSet<Credential> Credentials
        {
            get; set;
        }

    }
}


