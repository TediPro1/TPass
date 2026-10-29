namespace TPass.Models.Tags;

public class CredentialTagsModel
{
    public int CredentialId { get; set; }
    public string Website { get; set; } = string.Empty;
    public List<int> SelectedTagIds { get; set; } = [];
    public List<TagModel> AvailableTags { get; set; } = [];
}
