namespace CodyTedrick.DiscordBot.Scrapers;

public interface IScraper
{
    public enum AccountEnum
    {
        Steam = 0,
        Epic = 1,
        PlayStation = 2
    }
    
    string BaseUrl{ get; }
    
    void GetDataFromUrl(AccountEnum account, string gamerTag);
}