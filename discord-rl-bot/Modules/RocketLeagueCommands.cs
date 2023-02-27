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

    public RocketLeagueCommands(IServiceProvider services)
    {
        db = services.GetRequiredService<CsharpiEntities>();
        config = services.GetRequiredService<IConfiguration>();
        scraper = services.GetRequiredService<IScraper>();
    }

    [SlashCommand("addinformation", "Lets add your Rocket League GamerId")]
    public async Task AddInformation(string trackerUrl)
    {
        var userId = Context.User.Id;
        var foundEntity = GetEntityAsync(userId);

        if (foundEntity.Result != null) {
            foundEntity.Result.Url = trackerUrl;
            await UpdateEntityAsync(foundEntity.Result);
        }
        else
            await AddEntityAsync(userId, trackerUrl);

        await ReplyAsync($"Vroom Vroom, {Context.User.Username}! Try /ShowStats next...");
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
                // new() {Name = "RL Profile", Value = entry?}
            }
        };

        await ReplyAsync(null, false, embed.Build());
    }
    
    [SlashCommand("showstats", "Show off those hot stats")]
    public async Task ShowStats()
    {
        var userId = Context.User.Id;
        var entry = await GetEntityAsync(userId);
        
        // var account = entry.AccountType;
        var url = entry.Url;
        
        var sb = scraper.GetDataFromUrl(url);
        
        var embed = new EmbedBuilder {
            Color = Color.Blue,
            Title = $"{Context.User.Username}'s Rocket League Stats!",
            Description = sb.ToString()
        };
        
        await ReplyAsync(null,false, embed.Build());
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

    private async Task AddEntityAsync(ulong userId, string url)
    {
        await db.AddAsync(new UserInfo {UserId = userId, Url = url});
        await db.SaveChangesAsync();
    }
}