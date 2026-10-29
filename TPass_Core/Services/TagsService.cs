using Microsoft.EntityFrameworkCore;
using TPass_Core.Interfaces;
using TPass_Data.Data;
using TPass_Data.Entities;

namespace TPass_Core.Services;

public class TagsService(ApplicationDbContext context) : ITagsService
{
    private IQueryable<Tag> OwnedTags(string userId) => context.Tags.Where(t => t.UserId == userId);

    public Task<List<Tag>> GetAllAsync(string userId) => OwnedTags(userId).OrderBy(t => t.Name).ToListAsync();

    public Task<Tag?> GetByIdAsync(string userId, int id) => OwnedTags(userId).FirstOrDefaultAsync(t => t.Id == id);

    public async Task CreateAsync(string userId, Tag tag)
    {
        tag.UserId = userId;
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(string userId, Tag updated)
    {
        var tag = await OwnedTags(userId).FirstOrDefaultAsync(t => t.Id == updated.Id);
        if (tag is null) return false;
        tag.Name = updated.Name;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string userId, int id)
    {
        var tag = await OwnedTags(userId).FirstOrDefaultAsync(t => t.Id == id);
        if (tag is null) return false;
        context.Tags.Remove(tag);
        await context.SaveChangesAsync();
        return true;
    }

    public Task<Credential?> GetCredentialWithTagsAsync(string userId, int credentialId) => context.Credentials
        .Include(c => c.CredentialTags)
        .SingleOrDefaultAsync(c => c.Id == credentialId && c.UserId == userId && !c.IsDeleted);

    public async Task<bool> SetCredentialTagsAsync(string userId, int credentialId, IEnumerable<int> tagIds)
    {
        var credential = await context.Credentials
            .Include(c => c.CredentialTags)
            .SingleOrDefaultAsync(c => c.Id == credentialId && c.UserId == userId && !c.IsDeleted);
        if (credential is null) return false;

        var requestedIds = tagIds.Distinct().ToList();
        var ownedIds = await OwnedTags(userId)
            .Where(t => requestedIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();
        if (ownedIds.Count != requestedIds.Count) return false;

        context.CredentialTags.RemoveRange(credential.CredentialTags);
        context.CredentialTags.AddRange(ownedIds.Select(tagId => new CredentialTag
        {
            CredentialId = credentialId,
            TagId = tagId
        }));
        await context.SaveChangesAsync();
        return true;
    }
}
