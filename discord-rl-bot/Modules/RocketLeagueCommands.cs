using CodyTedrick.DiscordBot.Database;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodyTedrick.DiscordBot.Modules;

public class RocketLeagueCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly CsharpiEntities db;
    private readonly IConfiguration config;
    
    public RocketLeagueCommands(IServiceProvider services)
    {
        db = services.GetRequiredService<CsharpiEntities>();
        config = services.GetRequiredService<IConfiguration>();
    }

    [SlashCommand("addtag", "Lets add your Rocket League GamerId")]
    public async Task AddGamerTag(string gamerId)
    {
        var userId = Context.User.Id;
        var foundEntity = GetEntityAsync(userId);

        if (foundEntity.Result != null) {
            foundEntity.Result.GamerTag = gamerId;
            await UpdateEntityAsync(foundEntity.Result);
        }
        else
            await AddEntityAsync(userId, gamerId);

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

    private async Task AddEntityAsync(ulong userId, string gamerId)
    {
        await db.AddAsync(new UserInfo {UserId = userId, GamerTag = gamerId});
        await db.SaveChangesAsync();
    }
}