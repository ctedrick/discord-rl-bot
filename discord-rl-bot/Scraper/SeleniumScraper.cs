using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CodyTedrick.DiscordBot.Scrapers;

public class SeleniumScraper : IScraper
{
    public void GetDataFromUrl(string userUrl)
    {
        var options = new ChromeOptions();
        options.AddArguments("--headless", "--disable-web-security", "start-maximized"); // headless does not work...
        var driver = new ChromeDriver();
        
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        
        // Open the website
        driver.Navigate().GoToUrl(userUrl);
        
        // Wait for the player's profile to load
        Thread.Sleep(10000);
        
        // Find the elements containing the data you want to scrape
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        var name = wait.Until(d => d.FindElement(By.CssSelector(".trn-ign__username")));
        
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

        Console.WriteLine($"{name.Text}");

        foreach (var dataCategory in dataCategories) {
            var category = driver.FindElement(By.CssSelector($"[title*='{dataCategory}']"));
            var data = category.FindElement(By.XPath("following-sibling::*[1]"));
            Console.WriteLine($"{dataCategory}: {data.Text}");
        }

        // Close the browser
        driver.Quit();
    }
}