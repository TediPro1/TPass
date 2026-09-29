using System.ComponentModel.DataAnnotations;

namespace TPass.ViewModels.Credentials;

public class CredentialsEdit
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Website { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required, StringLength(50), DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
