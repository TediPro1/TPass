using System.ComponentModel.DataAnnotations;

namespace TPass.Models.Tags;

public class TagModel
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;
}
