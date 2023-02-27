using System.ComponentModel.DataAnnotations;

namespace CodyTedrick.DiscordBot.Database;

public class UserInfo
{
    [Key]
    public ulong UserId{ get; set; }
    public string? Url{ get; set; }
    
    // [Required]
    // public virtual int AccountTypeId
    // {
    //     get => (int)AccountType;
    //     set => AccountType = (IScraper.AccountEnum)value;
    // }
    //
    // [EnumDataType(typeof(IScraper.AccountEnum))]
    // public IScraper.AccountEnum AccountType { get; set; }
}