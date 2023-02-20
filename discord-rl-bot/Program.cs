using CodyTedrick.DiscordBot;

public class Program
{
    public static void Main(string[] args)
    {
        var bot = new Bot();
        bot.RunAsync().GetAwaiter().GetResult();
        
        // new Scraper().DisplayGamerData("https://rocketleague.tracker.network/rocket-league/profile/steam/76561198007099996/overview");
    }
}
