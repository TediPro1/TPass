namespace TPass.Models.Credentials;

public class CredentialsDelete
{
    public int Id { get; set; }
    public string Website { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
