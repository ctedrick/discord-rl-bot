using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CodyTedrick.DiscordBot.Scrapers;

public class SeleniumScraper : IScraper
{
    public string BaseUrl => "https://tracker.gg/rocket-league";

    public StringBuilder GetDataFromUrl(string url)
    {
        var options = new ChromeOptions();
        options.AddArguments("--headless", "--disable-web-security", "start-maximized"); // headless does not work...
        var driver = new ChromeDriver();
        
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        
        // Open the website
        driver.Navigate().GoToUrl(url);
        
        // // Find the elements containing the data you want to scrape
        // var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        // var name = wait.Until(d => d.FindElement(By.CssSelector(".platforms-dropdown.dropdown.dropdown--trigger")));
        // name.Click();
        //
        // var parent = wait.Until(d => d.FindElement(By.ClassName("dropdown__items")));
        //
        // // Find the elements containing the data you want to scrape
        // var labels = wait.Until(_ => parent.FindElements(By.ClassName("dropdown__item-label")));
        // foreach (var label in labels) {
        //     Console.WriteLine($"{label.Text}");
        //     if (label.Text == account.ToString())
        //         label.FindElement(By.XPath("ancestor::*[@class='dropdown__item']")).Click();
        // }
        //
        // name = wait.Until(d => d.FindElement(By.CssSelector($"[aria-label*='{account.ToString()}']")));
        // name.SendKeys(gamerTag);
        
        // Thread.Sleep(2000);
        //
        // var name = wait.Until(d => d.FindElement(By.CssSelector(".platforms-dropdown.dropdown.dropdown--trigger")));
        //
        // name.Submit();
        //
        // Thread.Sleep(10000);
        
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

        var sb = new StringBuilder();
        
        foreach (var dataCategory in dataCategories) {
            var category = driver.FindElement(By.CssSelector($"[title*='{dataCategory}']"));
            var data = category.FindElement(By.XPath("following-sibling::*[1]"));
            Console.WriteLine($"{dataCategory}: {data.Text}");
            sb.AppendLine($"{dataCategory}: {data.Text}");
        }
        
        // Close the browser
        driver.Quit();

        return sb;
    }
}