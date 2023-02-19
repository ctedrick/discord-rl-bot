using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace CodyTedrick.DiscordBot;

public class Bot
{
    public DiscordSocketClient Client{ get; private set; }
    
    public async Task RunAsync()
    {
        var token = JsonConvert.DeserializeObject<ConfigJson>(await File.ReadAllTextAsync("config.json")).Token;
   
        Client = new DiscordSocketClient();

        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();
        
        Client.Ready += ClientOnReady;
        Client.Log += Log;

        await Task.Delay(-1);
    }

    private static Task ClientOnReady()
    {
        Console.WriteLine("Bot is connected!");
        return Task.CompletedTask;
    }
    
    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}