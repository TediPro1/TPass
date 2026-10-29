using TPass_Data.Entities;

namespace TPass_Core.Interfaces;

public interface ITagsService
{
    Task<List<Tag>> GetAllAsync(string userId);
    Task<Tag?> GetByIdAsync(string userId, int id);
    Task CreateAsync(string userId, Tag tag);
    Task<bool> UpdateAsync(string userId, Tag tag);
    Task<bool> DeleteAsync(string userId, int id);
    Task<Credential?> GetCredentialWithTagsAsync(string userId, int credentialId);
    Task<bool> SetCredentialTagsAsync(string userId, int credentialId, IEnumerable<int> tagIds);
}
