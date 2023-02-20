using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CodyTedrick.DiscordBot;

public class Scraper
{
    public void DisplayGamerData(string userUrl)
    {
        var driver = new ChromeDriver();
        
        // Open the website
        driver.Navigate().GoToUrl(userUrl);
        
        // Wait for the player's profile to load
        Thread.Sleep(5000);
        
        // Find the elements containing the data you want to scrape
        IWebElement playerName = driver.FindElement(By.CssSelector(".trn-ign__username"));

        var dataCategories = new List<string>() {
            "Wins",
            "Goals",
            "Goal Shot Ratio",
            "Shots",
            "Assists",
            "Saves",
            "MVPs",
            "TRN Score",
        };

        Console.WriteLine($"{playerName.Text}");

        foreach (var dataCategory in dataCategories) {
            var goals = driver.FindElement(By.CssSelector($"[title*='{dataCategory}']"));
            var data = goals.FindElement(By.XPath("following-sibling::*[1]"));
            Console.WriteLine($"{dataCategory}: {data.Text}");
        }

        // Close the browser
        driver.Quit();
    }
}