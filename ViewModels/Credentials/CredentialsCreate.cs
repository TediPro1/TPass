using System.ComponentModel.DataAnnotations;

namespace TPass.ViewModels.Credentials;

public class CredentialsCreate
{
    [Required, StringLength(100)]
    public string Website { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required, StringLength(50), DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
