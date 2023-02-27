using System.Text;
using CodyTedrick.DiscordBot.Database;
using CodyTedrick.DiscordBot.Scrapers;
using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodyTedrick.DiscordBot.Modules;

public class RocketLeagueCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly CsharpiEntities db;
    private readonly IConfiguration config;
    private readonly IScraper scraper;

    private readonly List<IScraper.AccountEnum> accountChoices;

    public RocketLeagueCommands(IServiceProvider services)
    {
        db = services.GetRequiredService<CsharpiEntities>();
        config = services.GetRequiredService<IConfiguration>();
        scraper = services.GetRequiredService<IScraper>();
    }

    [SlashCommand("addinformation", "Lets add your Rocket League GamerId")]
    public async Task AddGamerTag
        (string gamerId, 
         [Choice("Steam", "Steam")]
         [Choice("Playstation", "Playstation")] string account)
    {
        var userId = Context.User.Id;
        var foundEntity = GetEntityAsync(userId);

        IScraper.AccountEnum acct;
        if (account == "Steam") 
            acct = IScraper.AccountEnum.Steam;
        else if (account == "Playstation")
            acct = IScraper.AccountEnum.PlayStation;
        else
            acct = IScraper.AccountEnum.Steam;

        if (foundEntity.Result != null) {
            foundEntity.Result.GamerTag = gamerId;
            foundEntity.Result.AccountType = acct;
            await UpdateEntityAsync(foundEntity.Result);
        }
        else
            await AddEntityAsync(userId, gamerId, acct);

        await ReplyAsync($"Vroom Vroom, {gamerId}!");
    }
    
    [SlashCommand("showaccountinfo", "View your account info")]
    public async Task ShowAccountInformation()
    {
        var userId = Context.User.Id;
        var entry = await GetEntityAsync(userId);

        var embed = new EmbedBuilder {
            Color = Color.Blue,
            Description = "Account Information",
            Fields = new List<EmbedFieldBuilder> {
                new() {Name = "User Name", Value = Context.User.Username},
                new() {Name = "Account Type", Value = entry?.AccountType.ToString()},
                new() {Name = "Gamer Tag", Value = entry?.GamerTag}
            }
        };

        await ReplyAsync(null, false, embed.Build());
    }
    
    [SlashCommand("showstats", "Show off those hot stats")]
    public async Task ShowStats()
    {
        var userId = Context.User.Id;
        var entry = await GetEntityAsync(userId);

        var account = entry.AccountType;
        var tag = entry.GamerTag;
        
        scraper.GetDataFromUrl(account, tag);
        
        await ReplyAsync(null, false, null);
    }

    private async Task<UserInfo?> GetEntityAsync(ulong id)
    {   
        return await db.UserInfo.FindAsync(id);
    }

    private async Task UpdateEntityAsync(UserInfo ui)
    {
        db.Entry(ui).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }

    private async Task AddEntityAsync(ulong userId, string gamerId, IScraper.AccountEnum accountEnum)
    {
        await db.AddAsync(new UserInfo {UserId = userId, GamerTag = gamerId, AccountType = accountEnum});
        await db.SaveChangesAsync();
    }
}