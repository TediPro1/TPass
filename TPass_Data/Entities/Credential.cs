using System.ComponentModel.DataAnnotations.Schema;

namespace TPass_Data.Entities
{
    public class Credential
    {
        public int Id { get; set; }
        public string Website { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId {  get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}


