namespace TPass_Data.Entities;
public class CredentialTag
{
 public int CredentialId { get; set; }
 public Credential Credential { get; set; } = null!;
 public int TagId { get; set; }
 public Tag Tag { get; set; } = null!;
}
