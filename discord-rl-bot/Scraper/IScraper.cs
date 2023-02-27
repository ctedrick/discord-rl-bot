using System.Text;

namespace CodyTedrick.DiscordBot.Scrapers;

public interface IScraper
{
    StringBuilder GetDataFromUrl(string url);
}