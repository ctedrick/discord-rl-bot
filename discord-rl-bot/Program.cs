using CodyTedrick.DiscordBot.Database;
using CodyTedrick.DiscordBot.Scrapers;
using CodyTedrick.DiscordBot.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private readonly IConfiguration config;
    private readonly IScraper scraper;
    
    private DiscordSocketClient client;
    private InteractionService commands;
    private readonly ulong testGuildId;

    public static Task Main()
    {
        return new Program().MainAsync();
    }

    public Program()
    {
        var cb = new ConfigurationBuilder()
                       .SetBasePath(AppContext.BaseDirectory)
                       .AddJsonFile(path: "config.json");
        config = cb.Build();
        testGuildId = ulong.Parse(config["TestGuildId"] ?? string.Empty);

        scraper = new SeleniumScraper();
    }
    
    public async Task MainAsync()
    {
        using (var services = ConfigureServices())
        {
            // get the client and assign to client 
            // you get the services via GetRequiredService<T>
            var client = services.GetRequiredService<DiscordSocketClient>();
            var commands = services.GetRequiredService<InteractionService>();
            this.client = client;
            this.commands = commands;

            // setup logging and the ready event
            client.Log += LogAsync;
            commands.Log += LogAsync;
            client.Ready += ReadyAsync;

            // this is where we get the Token value from the configuration file, and start the bot
            await client.LoginAsync(TokenType.Bot, config["Token"]);
            await client.StartAsync();

            // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
            await services.GetRequiredService<CommandHandler>().InitializeAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
    
    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;
    }

    private async Task ReadyAsync()
    {
        if (IsDebug()) {
            // this is where you put the id of the test discord guild
            Console.WriteLine($"Connected as -> [{client.CurrentUser}] :)");
            await commands.RegisterCommandsToGuildAsync(testGuildId);
        }
        // else {
        //     // this method will add commands globally, but can take around an hour
        //     await commands.RegisterCommandsGloballyAsync(true);
        // }
    }

    private ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
               .AddSingleton(config)
               .AddSingleton(scraper)
               .AddSingleton<DiscordSocketClient>()
               .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
               .AddSingleton<CommandHandler>()
               .AddDbContext<CsharpiEntities>()
               .BuildServiceProvider();
    }
    
    static bool IsDebug ( )
    {
        #if DEBUG
            return true;
        #else
            return false;
        #endif
    }
}
