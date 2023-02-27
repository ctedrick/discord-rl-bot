using CodyTedrick.DiscordBot.Database;
using CodyTedrick.DiscordBot.Scrapers;
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

    [SlashCommand("addtag", "Lets add your Rocket League GamerId")]
    public async Task AddGamerTag
        (string gamerId, 
         [Choice("Steam", "Steam")]
         [Choice("Playstation", "Playstation")] string option)
    {
        var userId = Context.User.Id;
        var foundEntity = GetEntityAsync(userId);

        IScraper.AccountEnum acct;
        if (option == "Steam") 
            acct = IScraper.AccountEnum.Steam;
        else if (option == "Playstation")
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

        await ReplyAsync($"Thanks, {gamerId}!");
    }
    
    [SlashCommand("showstats", "Lets add your Rocket League GamerId")]
    public async Task ShowStats(string gamerId)
    {
        await ReplyAsync($"Thanks, {gamerId}!");
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