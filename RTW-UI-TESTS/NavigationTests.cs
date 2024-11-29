using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace RTW_UI_TESTS;

[TestFixture]
[Description("UI tests for the navigation bar")]
public class NavigationTests
{
    private readonly IWebDriver driver;

    public NavigationTests()
    {
        driver = Environment.BrowserType switch
        {
            BrowserType.Chrome => new ChromeDriver(),
            BrowserType.Firefox => new FirefoxDriver(),
            _ => new ChromeDriver(),
        };
    }

    [OneTimeSetUp]
    public void SetupNavigationTests() {
        driver.Manage().Window.FullScreen();
    }

    [OneTimeTearDown]
    public void TearDownNavigationTests()
    {
        driver.Close();
        driver.Quit();
    }

    [Test]
    [Description("The About link on the top navigation bar should navigate to the About page")]
    public void AboutPageLink()
    {
        // Navigate to home page
        driver.Url = Environment.BaseUrl;
        driver.Manage().Window.FullScreen();

        // Check the about link and click it
        IWebElement aboutLink = driver.FindElement(By.Id("nav-aboutLink"));
        Assert.That(aboutLink.Displayed && aboutLink.Enabled, Is.True);
        aboutLink.Click();

        // Assert navigation
        SeleniumHelpers.WaitUntil(() => driver.Url == Environment.AboutUrl, 3);
        Assert.That(driver.Url, Is.EqualTo(Environment.AboutUrl));
        IWebElement tmdbLogo = driver.FindElement(By.CssSelector("img[alt='The Movie Database logo']"));
        Assert.That(tmdbLogo.Displayed, Is.True);
    }

    [Test]
    [Description("The Home link on the top navigation bar should navigate to the Home page")]
    public void HomePageLink()
    {
        // Navigate to about page
        driver.Url = Environment.AboutUrl;
        driver.Manage().Window.FullScreen();

        // Check the home link and click it
        IWebElement homeLink = driver.FindElement(By.Id("nav-homeLink"));
        Assert.That(homeLink.Displayed && homeLink.Enabled, Is.True);
        homeLink.Click();

        // Assert navigation
        SeleniumHelpers.WaitUntil(() => driver.Url == Environment.BaseUrl, 3);
        Assert.That(driver.Url, Is.EqualTo(Environment.BaseUrl));
        IWebElement recommendButton = driver.FindElement(By.Id("home-recommendButton"));
        Assert.That(recommendButton.Displayed, Is.True);
    }
}