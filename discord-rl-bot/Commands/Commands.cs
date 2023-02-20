using Discord.Commands;
using Discord.Interactions;

namespace CodyTedrick.DiscordBot;

public class Commands : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task Ping()
    {
        await ReplyAsync("Pong");
    }
}