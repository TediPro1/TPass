using TPass_Data.Entities;

namespace TPass_Core.Interfaces;

public interface ICredentialsService
{
    Task<List<Credential>> GetAllAsync(string userId);
    Task<Credential?> GetByIdAsync(string userId, int id);
    Task CreateAsync(string userId, Credential credential);
    Task<bool> UpdateAsync(string userId, Credential credential);
    Task<bool> DeleteAsync(string userId, int id);
    Task<bool> ExistsAsync(string userId, int id);
}
