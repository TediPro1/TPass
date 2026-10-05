using Microsoft.AspNetCore.Identity;

namespace TPass_Data.Entities;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public string? DisplayName { get; set; }
    public ICollection<Credential> Credentials { get; set; } = new List<Credential>();
}
