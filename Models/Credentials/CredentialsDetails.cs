namespace TPass.Models.Credentials;

public class CredentialsDetails
{
    public int Id { get; set; }
    public string Website { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public List<string> Tags { get; set; } = [];
}
