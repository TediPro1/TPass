using Microsoft.EntityFrameworkCore;
using TPass_Core.Interfaces;
using TPass_Data.Data;
using TPass_Data.Entities;

namespace TPass_Core.Services;

public class CredentialsService(ApplicationDbContext context) : ICredentialsService
{
    private IQueryable<Credential> Owned(string userId) => context.Credentials
        .Where(c => c.UserId == userId && !c.IsDeleted);

    public Task<List<Credential>> GetAllAsync(string userId) => Owned(userId).OrderBy(c => c.Website).ToListAsync();

    public Task<Credential?> GetByIdAsync(string userId, int id) => Owned(userId).Include(c => c.CredentialTags).ThenInclude(ct => ct.Tag).FirstOrDefaultAsync(c => c.Id == id);

    public async Task CreateAsync(string userId, Credential credential)
    {
        credential.UserId = userId;
        credential.CreatedOn = DateTime.UtcNow;
        credential.IsDeleted = false;
        context.Credentials.Add(credential);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(string userId, Credential updated)
    {
        var credential = await Owned(userId).FirstOrDefaultAsync(c => c.Id == updated.Id);
        if (credential is null) return false;
        credential.Website = updated.Website;
        credential.Username = updated.Username;
        credential.Password = updated.Password;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string userId, int id)
    {
        var credential = await Owned(userId).FirstOrDefaultAsync(c => c.Id == id);
        if (credential is null) return false;
        credential.IsDeleted = true;
        await context.SaveChangesAsync();
        return true;
    }

    public Task<bool> ExistsAsync(string userId, int id) => Owned(userId).AnyAsync(c => c.Id == id);
}
