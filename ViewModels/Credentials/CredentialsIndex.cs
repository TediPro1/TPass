using System.ComponentModel.DataAnnotations;

namespace TPass.ViewModels.Credentials;

public class CredentialsIndex
{
    public int Id { get; set; }
    public string Website { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
