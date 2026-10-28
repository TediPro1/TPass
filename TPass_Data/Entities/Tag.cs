namespace TPass_Data.Entities;
public class Tag
{
 public int Id { get; set; }
 public string Name { get; set; } = string.Empty;
 public string UserId { get; set; } = string.Empty;
 public User User { get; set; } = null!;
 public ICollection<CredentialTag> CredentialTags { get; set; } = new List<CredentialTag>();
}
