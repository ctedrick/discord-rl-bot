using System.ComponentModel.DataAnnotations;

namespace CodyTedrick.DiscordBot.Database;

public class UserInfo
{
    [Key]
    public long UserId{ get; set; }
    public string? GamerTag{ get; set; }
}