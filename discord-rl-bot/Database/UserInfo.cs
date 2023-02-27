using System.ComponentModel.DataAnnotations;
using CodyTedrick.DiscordBot.Scrapers;

namespace CodyTedrick.DiscordBot.Database;

public class UserInfo
{
    [Key]
    public ulong UserId{ get; set; }
    public string? GamerTag{ get; set; }
    
    [Required]
    public virtual int AccountTypeId
    {
        get => (int)AccountType;
        set => AccountType = (IScraper.AccountEnum)value;
    }
    
    [EnumDataType(typeof(IScraper.AccountEnum))]
    public IScraper.AccountEnum AccountType { get; set; }
}