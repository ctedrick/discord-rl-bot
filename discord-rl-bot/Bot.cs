using System.Reflection;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CodyTedrick.DiscordBot;

public class Bot
{
    public DiscordSocketClient Client{ get; private set; }
    public CommandService Commands{ get; private set; }
    
    public IServiceProvider Services{ get; set; }
    
    private ConfigJson config;
    
    public async Task RunAsync()
    {
        config = JsonConvert.DeserializeObject<ConfigJson>(await File.ReadAllTextAsync("config.json"));

        var socketConfig = new DiscordSocketConfig() {
            GatewayIntents = GatewayIntents.All
        };

        Client = new DiscordSocketClient(socketConfig);
        Commands = new CommandService();

        Services = new ServiceCollection()
                   .AddSingleton(Client)
                   .AddSingleton(Commands)
                   .BuildServiceProvider();
        
        Client.Log += Log;
        Client.Ready += ClientOnReady;

        await RegisterCommandsAsync();
        
        await Client.LoginAsync(TokenType.Bot, config.Token);
        await Client.StartAsync();

        await Task.Delay(-1);
    }

    public async Task RegisterCommandsAsync()
    {
        Client.MessageReceived += ClientOnMessageReceived;
        await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
    }

    private async Task ClientOnMessageReceived(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        var context = new SocketCommandContext(Client, message);
        if (message.Author.IsBot) 
            return;

        int msgPos = 0;
        if (message.HasStringPrefix("!", ref msgPos)) {
            var result = await Commands.ExecuteAsync(context, msgPos, Services);
            
            var newMessage = message.Content.Remove(0, 1);
            new Scraper().DisplayGamerData(newMessage);

            if (!result.IsSuccess) {
                Console.WriteLine(result.ErrorReason);
            }
        }
    }

    private async Task ClientOnReady()
    {
        Console.WriteLine("Bot is connected!");
        
        Client.SlashCommandExecuted += ClientOnSlashCommandExecuted;
    }

    private async Task ClientOnSlashCommandExecuted(SocketSlashCommand command)
    {
        await command.RespondAsync($"You executed {command.Data.Name}");
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}